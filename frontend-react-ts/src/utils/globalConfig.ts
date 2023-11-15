import { PATH_DASHBOARD, PATH_PUBLIC } from "../routes/paths";

//URLS
export const HOST_API_KEY = "https://localhost:7120/api";
export const REGISTER_URL = "/Auth/Register";
export const LOGIN_URL = "/Auth/Login";
export const ME_URL = "/Auth/me";
export const USERS_LIST_URL = "/Auth/users";
export const UPDATE_ROLE_URL = "/Auth/update-role";
export const USERNAMES_LIST_URL = "/Auth/usernames";
export const ALL_MESSAGES_URL = "/Message";
export const CREATE_MESSAGE_URL = "/Message/create";
export const MY_MESSAGE_URL = "/Message/mine";
export const LOGS_URL = "/Log";
export const MY_LOGS_URL = "/Log/mine";

//Auth Routes
export const PATH_AFTER_REGISTER = PATH_PUBLIC.login;
export const PATH_AFTER_LOGIN = PATH_DASHBOARD.dashboard;
export const PATH_AFTER_LOGOUT = PATH_PUBLIC.home;
