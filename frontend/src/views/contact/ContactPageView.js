import "./ContactPageStyles.css";
import ContactPageComponent from "../../components/UnspecifiedRoles/contact/ContactPageComponent.js";
import {Navigate} from "react-router-dom";

const ContactPageView = () => {
  return (
    <div className="container">
        <div className="cstmpadd">
            <ContactPageComponent/>
        </div>
    </div>
  );
};

export default ContactPageView;
