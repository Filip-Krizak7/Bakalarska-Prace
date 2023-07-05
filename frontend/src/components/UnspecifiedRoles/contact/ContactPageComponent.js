import React, { useEffect, useState } from 'react';
import "./ContactPageComponent.css";
import { BsAt, BsLink, BsFillPersonFill } from "react-icons/bs";


const ContactPageComponent = () => {
    let iconStyles = { fontSize: "1.35em", marginRight: "10px" };
    
    return (
        <div style={{ marginTop: "30px" }}>
            <div className="row">
                <div className="col-sm">
                    <h1>Kontakty</h1>
                    <p style={{ paddingTop: "25px" }}><BsFillPersonFill style={iconStyles} /><b>Jméno:</b> Marek Vajgl </p>
                    <p><b><BsAt style={iconStyles} />E-mail:</b> marek.vajgl@osu.cz</p> 
                    <p>
                      <b><BsLink style={iconStyles} />Web:</b>{" "}
                      <a href="https://www.osu.cz/" target="_blank" rel="noopener noreferrer">
                        https://www.osu.cz/
                      </a>
                    </p>                         
                </div>
                <div className="col-sm uploadCol"></div>
            </div>
        </div>
    )
}

export default ContactPageComponent;