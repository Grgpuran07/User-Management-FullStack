using Backend.Core.Constants;
using Backend.Core.Dtos.Auth;
using Backend.Core.Dtos.General;
using Backend.Core.Entities;
using Backend.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace Backend.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogService _logService;
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration, ILogService logService, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _logService = logService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<GeneralServiceResponseDto> SeedRolesAsync()
        {
            bool isOwnerRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.OWNER);
            bool isAdminRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.ADMIN);
            bool isManagerRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.MANAGER);
            bool isUserRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.USER);

            if (isOwnerRoleExists && isAdminRoleExists && isManagerRoleExists && isUserRoleExists)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSucceed = true,
                    StatusCode = 200,
                    Message = "Role seeding is already done."
                };

            }

            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.OWNER));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.ADMIN));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.MANAGER));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.USER));

            return new GeneralServiceResponseDto()
            {
                IsSucceed = true,
                StatusCode = 200,
                Message = "Role seeding done sucessfully."
            };
        }

        public async Task<GeneralServiceResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var isExistsUser = await _userManager.FindByNameAsync(registerDto.UserName);
            if (isExistsUser is not null) {
                return new GeneralServiceResponseDto() { 
                   IsSucceed = false,
                   StatusCode = 409,
                   Message = "UserName Already Exists"
                };
            }

            ApplicationUser newUser = new ApplicationUser() { 
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                Address = registerDto.Address,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var createResult = await _userManager.CreateAsync(newUser,registerDto.Password);
            if (!createResult.Succeeded) {
                var errorString = "User Creation failed because: ";
                foreach (var error in createResult.Errors)
                {
                    errorString += " # " + error.Description;
                }

                return new GeneralServiceResponseDto()
                {
                    IsSucceed = false,
                    StatusCode = 200,
                    Message = errorString
                };
            }

            //Add a default user role to all users
            await _userManager.AddToRoleAsync(newUser,StaticUserRoles.USER);
            await _logService.SaveNewLog(newUser.UserName,"Registered to Website.");

            return new GeneralServiceResponseDto() { 
              IsSucceed = true,
              StatusCode = 201,
              Message = "User Created Sucessfully."
            };
        }

        public async Task<LoginServiceResponseDto?> LoginAsync(LoginDto loginDto)
        {
            //Find user by UserName
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user is null) {
                return null;
            }

            //Check Password of User
            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user,loginDto.Password);

            if (!isPasswordCorrect)
                return null;

            //Return token and userinfo to frontend
            var newToken = await GenerateJWTTokenAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var userinfo = GenerateUserInfoObject(user,roles);
            await _logService.SaveNewLog(user.UserName,"New Login");

            return new LoginServiceResponseDto()
            {
                NewToken = newToken,
                UserInfo = userinfo
            };
        }

        public async Task<GeneralServiceResponseDto> UpdateRoleAsync(ClaimsPrincipal User, UpdateRoleDto updateRoleDto)
        {
            //Find user by UserName
            var user = await _userManager.FindByNameAsync(updateRoleDto.UserName);
            if(user is null)
                return new GeneralServiceResponseDto()
                {
                    IsSucceed = false,
                    StatusCode = 404,
                    Message = "Invalid User."
                };

            var userRoles = await _userManager.GetRolesAsync(user);

            if (User.IsInRole(StaticUserRoles.ADMIN))
            {
                //User is admin
                if (updateRoleDto.NewRole == RoleType.USER || updateRoleDto.NewRole == RoleType.MANAGER)
                {
                    //Admin can change the role of everyone except for admins and owner
                    if (userRoles.Any(q => q.Equals(StaticUserRoles.OWNER) || userRoles.Any(q => q.Equals(StaticUserRoles.ADMIN))))
                    {
                        return new GeneralServiceResponseDto()
                        {
                            IsSucceed = false,
                            StatusCode = 403,
                            Message = "You are not allowed to change the role of this user."
                        };
                    }
                    else
                    {
                        await _userManager.RemoveFromRolesAsync(user, userRoles);
                        await _userManager.AddToRoleAsync(user, updateRoleDto.NewRole.ToString());
                        await _logService.SaveNewLog(user.UserName, "User Roles Updated.");
                        return new GeneralServiceResponseDto()
                        {
                            IsSucceed = true,
                            StatusCode = 200,
                            Message = "Role Updated Sucessfully"
                        };
                    }
                }
                else
                {
                    return new GeneralServiceResponseDto()
                    {
                        IsSucceed = false,
                        StatusCode = 403,
                        Message = "You are not allowed to change the role of this user."
                    };
                }
            }
            else
            {
                //User is owner
                if (userRoles.Any(q => q.Equals(StaticUserRoles.OWNER)))
                {
                    return new GeneralServiceResponseDto()
                    {
                        IsSucceed = false,
                        StatusCode = 403,
                        Message = "You are not allowed to change the role of this user."
                    };
                }
                else
                {
                    await _userManager.RemoveFromRolesAsync(user, userRoles);
                    await _userManager.AddToRoleAsync(user, updateRoleDto.NewRole.ToString());
                    await _logService.SaveNewLog(user.UserName, "User Roles Updated.");
                    return new GeneralServiceResponseDto()
                    {
                        IsSucceed = true,
                        StatusCode = 200,
                        Message = "Role Updated Sucessfully"
                    };
                }
            }
             

        }

        public async Task<LoginServiceResponseDto?> MeAsync(MeDto meDto)
        {
            ClaimsPrincipal handler = new JwtSecurityTokenHandler().ValidateToken(meDto.Token, new TokenValidationParameters() { 
              ValidateIssuer = true,
              ValidateAudience = true,
              ValidIssuer = _configuration["JWT:ValidIssuer"],
              ValidAudience = _configuration["JWT:ValidAudience"],
              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]))
            },out SecurityToken securityToken);

            string decodedUserName = handler.Claims.First(q => q.Type == ClaimTypes.Name).Value;
            if (decodedUserName is null)
                return null;

            var user = await _userManager.FindByNameAsync(decodedUserName);
            if (user == null) return null;

            var newToken = await GenerateJWTTokenAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var userinfo = GenerateUserInfoObject(user, roles);
            await _logService.SaveNewLog(user.UserName, "New Token Generated");

            return new LoginServiceResponseDto() { 
              NewToken = newToken,
              UserInfo =userinfo,
            };
        }

        public async Task<IEnumerable<UserInfoResult>> GetUserListAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            List<UserInfoResult> userInfoResults = new List<UserInfoResult>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userInfo = GenerateUserInfoObject(user, roles);
                userInfoResults.Add(userInfo);
            }

            return userInfoResults;
        }

        public async Task<UserInfoResult?> GetUserDetailsByUserNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) return null;
            var roles = await _userManager.GetRolesAsync(user);
            var userinfo = GenerateUserInfoObject(user, roles);
            return userinfo;
        }

        public async Task<IEnumerable<string>> GetUsersNameListAsync()
        {
            var userNames = await _userManager.Users.Select(q=>q.UserName).ToListAsync();

            return userNames;
        }


        private async Task<string> GenerateJWTTokenAsync(ApplicationUser user) {
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>() {
              new Claim(ClaimTypes.Name,user.UserName),
              new Claim(ClaimTypes.NameIdentifier,user.Id),
              new Claim("FirstName",user.FirstName),
              new Claim("LastName",user.LastName)
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role,userRole));
            }

            var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var signInCredentials = new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256);

            var tokenObject = new JwtSecurityToken(
                 issuer:_configuration["JWT:ValidIssuer"],
                 audience:_configuration["JWT:ValidAudience"],
                 notBefore:DateTime.Now,
                 expires:DateTime.Now.AddHours(3),
                 claims:authClaims,
                 signingCredentials:signInCredentials
                );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

            return token;
            
        }

        private UserInfoResult GenerateUserInfoObject(ApplicationUser user,IEnumerable<string> Roles) {

            return new UserInfoResult() {
              Id = user.Id,
              FirstName = user.FirstName,
              LastName = user.LastName,
              UserName = user.UserName,
              Email = user.Email,
              CreatedAt = user.CreatedAt,
              Roles = Roles
            };
        }

         
    }
}
