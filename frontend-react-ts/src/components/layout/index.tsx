import Header from "./Header";
import Sidebar from "./Sidebar";
import { Outlet, useLocation } from "react-router-dom";
import useAuth from "../../hooks/useAuth.hooks";

const Layout = () => {
  const { isAuthenticated } = useAuth();
  const { pathname } = useLocation();

  console.log("pathname ", pathname);

  const sideBarRendered = () => {
    if (isAuthenticated && pathname.toLowerCase().startsWith("/dashboard")) {
      return <Sidebar />;
    }

    return null;
  };

  return (
    <div>
      <Header />
      {/* Using outlet, We render all routes that are inside of this Layout */}
      <div className="flex">
        {sideBarRendered()}
        <Outlet />
      </div>
    </div>
  );
};

export default Layout;
