import useAuth from "../../hooks/useAuth.hooks";
import Button from "../general/Button";
import { AiOutlineHome } from "react-icons/ai";
import { FiLock, FiUnlock } from "react-icons/fi";
import { useNavigate } from "react-router-dom";
import { PATH_DASHBOARD, PATH_PUBLIC } from "../../routes/paths";

const Header = () => {
  const { isAuthloading, isAuthenticated, user, logout } = useAuth();
  const navigate = useNavigate();

  const userRolesLabelCreator = () => {
    if (user) {
      let result = "";
      user.roles.forEach((role, index) => {
        result += role;
        if (index < user.roles.length - 1) {
          result += ", ";
        }
      });
      return result;
    }
    return "--";
  };
  console.log("isAuthenticated ", isAuthenticated);
  return (
    <div className="flex justify-between items-center bg-[#f0ecf7] h-12 px-4">
      <div className="flex items-center gap-4">
        <AiOutlineHome
          className="w-8 h-8 text-purple-500 hover:text-purple-700 cursor-pointer"
          onClick={() => {
            navigate("/");
          }}
        />
        <div className="flex gap-1">
          <h1 className="px-1 border border-dashed border-purple-300 rounded-lg">
            AuthLoading: {isAuthloading ? "True" : "--"}
          </h1>
          <h1 className="px-1 border border-dashed border-purple-300 rounded-lg flex items-center gap-1">
            Auth:{" "}
            {isAuthenticated ? (
              <FiUnlock className="text-green-600" />
            ) : (
              <FiUnlock className="text-red-600" />
            )}
          </h1>
          <h1 className="px-1 border border-dashed border-purple-300 rounded-lg">
            UserName : {user ? user.userName : "--"}
          </h1>
          <h1 className="px-1 border border-dashed border-purple-300 rounded-lg">
            UserRoles : {userRolesLabelCreator()}
          </h1>
        </div>
      </div>
      <div>
        {isAuthenticated ? (
          <div className="flex items-center gap-2">
            <Button
              label="Dashboard"
              type="button"
              variant="light"
              onClick={() => {
                navigate(PATH_DASHBOARD.dashboard);
              }}
            />
            <Button
              label="Logout"
              type="button"
              variant="light"
              onClick={logout}
            />
          </div>
        ) : (
          <div className="flex items-center gap-2">
            <Button
              label="Register"
              type="button"
              variant="light"
              onClick={() => {
                navigate(PATH_PUBLIC.register);
              }}
            />
            <Button
              label="Login"
              type="button"
              variant="light"
              onClick={() => {
                navigate(PATH_PUBLIC.login);
              }}
            />
          </div>
        )}
      </div>
    </div>
  );
};

export default Header;
