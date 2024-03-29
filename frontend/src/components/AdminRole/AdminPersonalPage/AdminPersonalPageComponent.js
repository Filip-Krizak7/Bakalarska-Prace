import React, {useEffect, useState} from 'react';
import {axios} from "../../../axios";
import "./AdminPersonalPageComponent.css";
import {BsAt, BsFillPersonFill, BsPhone, BsTools} from "react-icons/bs";
import ChangePasswordComponent from "../../UnspecifiedRoles/changePassword/ChangePasswordComponent";

const URL = `${process.env.REACT_APP_AXIOS_URL}`;
const GET_DATA_URL = `${URL}/user/data`;

const AdminPersonalPageComponent = () => {
    let iconStyles = {fontSize: "1.35em", marginRight: "10px"};
    const [name, setName] = useState("");
    const [email, setEmail] = useState("");
    const [phone, setPhone] = useState("");

    const getUserData = async () => {

        // Make first two requests
        const response = await Promise.all([
            axios({
                url: GET_DATA_URL,
                withCredentials: true,
                headers: { 'Authorization': localStorage.getItem("token") },
                method: "GET",
            }),
        ]);

        // Update state once with all 3 responses
        setName((response[0].data.firstName + " " + response[0].data.secondName));
        setEmail(response[0].data.username);
        setPhone(response[0].data.phoneNumber);
    };

    useEffect(() => {
        getUserData();
    }, []);

    const formatPhoneNum = (number) => {
        if(number === null || number === "") return;
        let ret = number.replaceAll(" ", "");
        let formattedNum = "";
        let index = 0;

        if (ret.substring(0,4) === "+420") {
            formattedNum = formattedNum.concat("+420 ");
            index += 4;
        }
        for (let i = 0; i < 3; i++) {
            formattedNum = formattedNum.concat(ret.substring(index, index + 3));
            if (i < 2) { formattedNum = formattedNum.concat(" "); }
            index += 3;
        }

        return formattedNum;
    }

    return (
        <div style={{marginTop: "30px"}}>
            <div className="row">
                <div className="col-sm">
                    <h1>Osobní stránka</h1>
                    <p style={{paddingTop: "25px"}}><BsFillPersonFill style={iconStyles}/><b>Jméno:</b> {name}</p>
                    <p><b><BsAt style={iconStyles}/>E-mail:</b> {email}</p>
                    <p><b><BsPhone style={iconStyles}/>Telefon</b>: {formatPhoneNum(phone)}</p>
                    <p><b><BsTools style={iconStyles}/>Změna hesla: <ChangePasswordComponent/></b></p>
                </div>
                <div className="col-sm uploadCol"></div>
            </div>
        </div>
    )
}

export default AdminPersonalPageComponent;