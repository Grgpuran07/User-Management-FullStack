import PageAccessTemplate from "../../components/dashboard/page-access/PageAccessTemplate";
import { FaUserShield } from "react-icons/fa";

const AdminPage = () => {
  return (
    <div className="pageTemplate2">
      <PageAccessTemplate
        color="#933ea"
        icon={FaUserShield}
        role="Admin"
      ></PageAccessTemplate>
    </div>
  );
};

export default AdminPage;
