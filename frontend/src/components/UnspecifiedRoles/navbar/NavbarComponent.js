import Navbar from "react-bootstrap/Navbar";
import { Container, Nav } from "react-bootstrap";
import React, { useEffect, useState } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import Logo from "../../../resources/OSUbile.png";
import "./NavbarStyles.css";
import { BsFillHouseFill, BsFillPersonFill, BsTelephoneFill } from "react-icons/bs";
import LoginControlComponent from "../login/logincontrol/LoginControlComponent";

let iconStyles = { fontSize: "2.5em" };

function refreshPage() {
    window.location.reload();
}

const NavbarComponent = () => {

    const redirectBasedOnRoleMainView = () => {
        console.log("role", JSON.parse(localStorage.getItem("user")).role);
        switch (JSON.parse(localStorage.getItem("user")).role) {
            case 'ROLE_STUDENT':
                return window.location.href = '/studentHome';
            case "ROLE_TEACHER":
                return window.location.href = '/teacherHome';
            case 'ROLE_COORDINATOR':
                return window.location.href = '/coordinatorHome';
            case 'ROLE_ADMIN':
                console.log("admin");
                return window.location.href = '/adminHome';
            default:
                console.log("fail");
                return window.location.href = '/login';
        }
    }

    const redirectBasedOnRolePersonalPage = () => {
        switch (JSON.parse(localStorage.getItem("user")).role) {
            case 'ROLE_STUDENT':
                return window.location.href = '/studentPersonal';
            case "ROLE_TEACHER":
                return window.location.href = '/teacherPersonal';
            case 'ROLE_COORDINATOR':
                return window.location.href = '/coordinatorPersonal';
            case 'ROLE_ADMIN':
                return window.location.href = '/adminPersonal';
            default:
                return window.location.href = '/login';
        }
    }

    const redirectOnContactPage = () => {
        return window.location.href = '/contact';
    }

    return (
        <div className="navbar-main">
            <Navbar>
                <div className="main-container">
                    <Navbar.Brand href="#" className="navbar-brand">
                        <img
                            onClick={refreshPage}
                            className="img-responsive"
                            src={Logo}
                            alt="logo"
                        />
                    </Navbar.Brand>
                    <Nav className="navbar-links">
                    {localStorage.getItem("token") && (
                        <Nav.Link className="nav-link">
                            <span
                            className="my-hover"
                            onClick={() => redirectBasedOnRoleMainView()}
                            id="prehled"
                            >
                            <BsFillHouseFill className="icon" style={iconStyles} />
                            <p className={"p-margin"}>Domů</p>
                            </span>
                        </Nav.Link>
                        )}
                        {localStorage.getItem("token") && (
                            <Nav.Link className="nav-link">
                                <span
                                className="my-hover"
                                onClick={() => redirectBasedOnRolePersonalPage()}
                                id="prehled"
                                >
                                <BsFillPersonFill className="icon" style={iconStyles} />
                                <p className={"p-margin"}>Účet</p>
                                </span>
                            </Nav.Link>
                            )}
                        {localStorage.getItem("token") ? (
                            <Nav.Link className="nav-link" style={{ marginLeft: "16px" }}>
                            <span
                                className="my-hover"
                                onClick={() => redirectOnContactPage()}
                                id="prehled"
                            >
                                <BsTelephoneFill className="icon" style={iconStyles} />
                                <p className={"p-margin"}>Kontakt</p>
                            </span>
                            </Nav.Link>
                        ) : (
                            <Nav.Link className="nav-link">
                            <span
                                className="my-hover"
                                onClick={() => redirectOnContactPage()}
                                id="prehled"
                            >
                                <BsTelephoneFill className="icon" style={iconStyles} />
                                <p className={"p-margin"}>Kontakt</p>
                            </span>
                            </Nav.Link>
                        )}
                    </Nav>
                    <Navbar.Collapse className="margin-left-cstm-nav">
                        <Nav>
                            <Nav eventkey={2} className="navbar-text white">
                                <LoginControlComponent />
                            </Nav>
                        </Nav>
                    </Navbar.Collapse>
                </div>
            </Navbar>
        </div>
    );
};

export default NavbarComponent;
