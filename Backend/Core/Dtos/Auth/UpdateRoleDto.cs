using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Dtos.Auth
{
    public class UpdateRoleDto
    {
        [Required(ErrorMessage = "UserName is Required.")]
        public string UserName { get; set; }
        public RoleType NewRole { get; set; }
    }

    public enum RoleType { 
      USER,
      MANAGER,
      ADMIN
    }
}
