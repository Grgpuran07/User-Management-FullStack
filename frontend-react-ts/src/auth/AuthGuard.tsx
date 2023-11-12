import { Navigate, Outlet } from "react-router-dom";
import AuthSpinner from "../components/general/AuthSpinner";
import { PATH_PUBLIC } from "../routes/paths";
import useAuth from "../hooks/useAuth.hooks";

//Interface for props
//We receive a roles array and we will decide the next step based on this array
interface IProps {
  roles: string[];
}

const AuthGuard = ({ roles }: IProps) => {
  const { isAuthenticated, user, isAuthloading } = useAuth();
  //Do we have access to the requested page(the page will be rendered in the <Outlet/>)
  const hasAccess =
    isAuthenticated && user?.roles?.find((q) => roles.includes(q));
  if (isAuthloading) {
    return <AuthSpinner />;
  }

  return hasAccess ? <Outlet /> : <Navigate to={PATH_PUBLIC.unauthorized} />;
};

export default AuthGuard;
