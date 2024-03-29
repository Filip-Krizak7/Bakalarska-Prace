import axios from "axios";

const LOGIN_URL = `${process.env.REACT_APP_AXIOS_URL}/login`;
const REGISTER_URL = `${process.env.REACT_APP_AXIOS_URL}/register`;
const REGISTER_COORDINATOR_URL = `${process.env.REACT_APP_AXIOS_URL}/coordinator/registerCoordinator`;
const CONFIRMATION_URL = `${process.env.REACT_APP_AXIOS_URL}/register/confirm?`;
const CHANGE_PASSWORD_URL = `${process.env.REACT_APP_AXIOS_URL}/user/changePassword`;
const EMAIL_FOR_RESET_URL = `${process.env.REACT_APP_AXIOS_URL}/forgotPassword/reset`;
const FORGOT_PASSWORD_URL = `${process.env.REACT_APP_AXIOS_URL}/forgotPassword/save`;

function getCookie(name) {
    const cookies = document.cookie.split(';');
    for (let i = 0; i < cookies.length; i++) {
        const cookie = cookies[i].trim();
        if (cookie.startsWith(name + '=')) {
            const token = cookie.substring(name.length + 1);
            return 'Bearer ' + token;
        }
    }
    return null;
}

class AuthService {
    login(username, password) {
        const formData = JSON.stringify({username, password});
        return axios({
            url: LOGIN_URL,
            withCredentials: true,
            method: "POST",
            data: formData,
            headers: {'Content-Type': 'application/json', 'Accept': 'application/json', 'Access-Control-Allow-Origin': 'http://localhost',
                'Authorization': localStorage.getItem("token") },
        }).then((response) => {
            if (response) {
                localStorage.setItem("user", JSON.stringify(response.data));
                const token = getCookie("access_token");
                localStorage.setItem("token", token);
                if (response.data.role === "ROLE_COORDINATOR") {
                    localStorage.setItem("role", "ROLE_COORDINATOR");
                } else if (response.data.role === "ROLE_STUDENT") {
                    localStorage.setItem("role", "ROLE_STUDENT");
                } else if (response.data.role === "ROLE_TEACHER") {
                    localStorage.setItem("role", "ROLE_TEACHER");
                } else if (response.data.role === "ROLE_ADMIN"){
                    localStorage.setItem("role", "ROLE_ADMIN");
                }
                window.location.reload();
            }
            return response.data;
        });
    }

    register(email, firstName, lastName, school, phoneNumber, password, role) {
        const formData = JSON.stringify({email, firstName, lastName, school, phoneNumber, password, role});

        return axios({
            url: REGISTER_URL,
            headers: {'content-type': 'application/json'},
            withCredentials: false,
            method: "POST",
            data: formData,
        }).then((response) => {
            if (response) {
            }
            return response.data;
        });
    }

    registerCoordinator(email, firstName, lastName, password, role) {
        const formData = JSON.stringify({email, firstName, lastName, password, role});

        return axios({
            url: REGISTER_COORDINATOR_URL,
            headers: {'content-type': 'application/json'}, //'Authorization': localStorage.getItem("token")
            withCredentials: true,
            method: "POST",
            data: formData,
        }).then((response) => {
            if (response) {
                window.location.reload();
            }
            return response.data;
        });
    }

    sendConfirmationToken(token) {
        return axios({
            url: CONFIRMATION_URL + token,
            withCredentials: false,
            method: "GET",
        }).then((response) => {
            if (response) {
                return response.data
            }
            return response.data;
        });
    }

    changePassword(oldPassword, newPassword) {
        const formData = JSON.stringify({oldPassword, newPassword});

        return axios({
            url: CHANGE_PASSWORD_URL,
            headers: {'content-type': 'application/json', 'Authorization': localStorage.getItem("token") },
            withCredentials: true,
            method: "POST",
            data: formData,
        }).then((response) => {
            if (response) {
            }
            return response.data;
        });
    }

    forgotPasswordEmail(email) {
        const data = {
            email: email
        };

        return axios({
            url: EMAIL_FOR_RESET_URL,
            headers: {'content-type': 'application/json'},
            withCredentials: false,
            method: "POST",
            data: data,
        }).then((response) => {
            if (response) {
                return response.data
            }
            return response.data;
        });
    }

    forgotPasswordAfterAuthorization(password, token) {
        let form = { "newPassword": password, "token": token };
        const data = new FormData();
        data.append('newPassword', password);
        data.append('token', token);

        return axios({
            url: FORGOT_PASSWORD_URL,
            headers: {'content-type': 'application/json',  }, //'Authorization': localStorage.getItem("token")
            withCredentials: true,
            method: "POST",
            data: form,
        }).then((response) => {
            if (response) {
            }
            return response.data;
        });
    }
}

export default new AuthService();
