import { CiUser } from "react-icons/ci";
import Button from "../general/Button";
import { useNavigate } from "react-router-dom";
import { PATH_DASHBOARD } from "../../routes/paths";
import useAuth from "../../hooks/useAuth.hooks";

const Sidebar = () => {
  const { user } = useAuth();
  const navigate = useNavigate();

  const handleClick = (url: string) => {
    window.scrollTo({ top: 0, left: 0, behavior: "smooth" });
    navigate(url);
  };

  return (
    <div className="shrink-0 bg-[#754eb4] w-60 p-2 min-h-[calc(100vh-48px)] flex flex-col items-stretch gap-8">
      <div className="self-center flex flex-col items-center">
        <CiUser className="w-10 h-10 text-white" />
        <h4 className="text-white">
          {user?.firstName} {user?.lastName}
        </h4>
      </div>
      <Button
        label="Users Management"
        type="button"
        variant="secondary"
        onClick={() => {
          handleClick(PATH_DASHBOARD.userManagement);
        }}
      />
      <Button
        label="Send Message"
        type="button"
        variant="secondary"
        onClick={() => {
          handleClick(PATH_DASHBOARD.sendMessage);
        }}
      />
      <Button
        label="Inbox"
        type="button"
        variant="secondary"
        onClick={() => {
          handleClick(PATH_DASHBOARD.inbox);
        }}
      />
      <Button
        label="All Messages"
        type="button"
        variant="secondary"
        onClick={() => {
          handleClick(PATH_DASHBOARD.allMessages);
        }}
      />
      <Button
        label="All Logs"
        type="button"
        variant="secondary"
        onClick={() => {
          handleClick(PATH_DASHBOARD.systemLogs);
        }}
      />
      <Button
        label="My Logs"
        type="button"
        variant="secondary"
        onClick={() => {
          handleClick(PATH_DASHBOARD.myLogs);
        }}
      />
      <hr />
      <Button
        label="Owner"
        type="button"
        variant="secondary"
        onClick={() => {
          handleClick(PATH_DASHBOARD.owner);
        }}
      />
      <Button
        label="Admin Page"
        type="button"
        variant="secondary"
        onClick={() => {
          handleClick(PATH_DASHBOARD.admin);
        }}
      />
      <Button
        label="Manager Page"
        type="button"
        variant="secondary"
        onClick={() => {
          handleClick(PATH_DASHBOARD.manager);
        }}
      />
      <Button
        label="User"
        type="button"
        variant="secondary"
        onClick={() => {
          handleClick(PATH_DASHBOARD.user);
        }}
      />
    </div>
  );
};

export default Sidebar;
