import PageAccessTemplate from "../../components/dashboard/page-access/PageAccessTemplate";
import { FaUserTie } from "react-icons/fa";

const ManagerPage = () => {
  return (
    <div className="pageTemplate2">
      <PageAccessTemplate
        color="#0b96bc"
        icon={FaUserTie}
        role="Manager"
      ></PageAccessTemplate>
    </div>
  );
};

export default ManagerPage;
