import { useContext } from "react";
import { AuthContext } from "../auth/auth.context";

const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context)
    throw new Error("useAuthContext is not inside of AuthProvider tag.");
  return context;
};

export default useAuth;
