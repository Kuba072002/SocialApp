import { React, useState } from 'react';
import { useParams, useNavigate } from "react-router-dom";
import axios from 'axios';
import { AiOutlineCheckCircle } from "react-icons/ai";
import apiconfig from "../../apiconfig.json"
const SignupSuccess = () => {
    const myStyle = {
        height: "100vh",
        display: "flex",
        alignItems: "center",
        flexDirection: "column",
        fontFamily: "Inter"
    };
    const myStyle2 = {
        color: "#FFF",
        marginTop: "1rem"
    };

    const buttonStyle = {
        marginTop: "1.5rem",
        padding: "0.5rem 1rem",
        color: "#fff",
        background: "#FF4820",
        fontFamily: "Inter",
        fontWeight: 400,
        fontSize: "18px",
        lineHeight: "25px",
        borderRadius: "5px",
        border: "0",
        outline: "none",
        cursor: "pointer"
    };

    const { token } = useParams();

    const [errorMessage, setErrorMessage] = useState('');
    const [successMessage, setSuccessMessage] = useState('');

    const [isSuccess, setIsSuccess] = useState(false);
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            //console.log(token);
            console.log(decodeURIComponent(token));
            await axios.post(`${apiconfig.API_KEY}Auth/confirmEmail`, {
                token: decodeURIComponent(token),
                purpose: "confirm_email"
            });
            
            setSuccessMessage('Email address confirmed. You can now log in.');
            setIsSuccess(true);
        } catch (error) {
            //console.log(JSON.stringify(error.response.data));
            setErrorMessage('Email address confirmation failed.')
        }
    };

    const handleReturnButton = (e) => {
        navigate("/login");
    };

    return (
        <div style={myStyle} className='gradient_bg'>
            <AiOutlineCheckCircle style={{ marginTop: "3rem" }} color="#4BB543" size={67} />
            <h2 style={myStyle2}>Registration success</h2>
            <button style={buttonStyle} onClick={handleSubmit}>Click here to confirm</button>
            <p style={{ marginTop: "0.5rem" }}>{errorMessage}</p>
            <p style={{color: 'white', marginTop:'0.5rem'}}>{successMessage}</p>
            {isSuccess && <button style={buttonStyle} onClick={handleReturnButton}>Login Page</button>}
        </div>
    )
}

export default SignupSuccess