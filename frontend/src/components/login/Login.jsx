import { React, useState, useEffect } from 'react'
import axios from 'axios';
import { useNavigate } from "react-router-dom";
import Dropzone from 'react-dropzone';
import "./login.css"

const Login = () => {

  const [sign, setsign] = useState("signin");

  const [signUpInput, setSignUpInput] = useState({
    firstName: '',
    lastName: '',
    birthDate: '',
    email: '',
    password: '',
    confirmPassword: '',
    picture: ''
  });

  const [signUpError, setSignUpError] = useState({
    password: '',
    confirmPassword: '',
    result: ''
  });

  const [signInInput, setSignInInput] = useState({
    email: '',
    password: ''
  });

  const onSignUpInputChange = e => {
    const { name, value } = e.target;
    setSignUpInput(inputs => ({
      ...inputs,
      [name]: value
    }));
    if (name === 'password' || name === 'confirmPassword')
      validateInput(e);
  };

  const onSignInInputChange = e => {
    const { name, value } = e.target;
    setSignInInput(inputs => ({
      ...inputs,
      [name]: value
    }));
  };

  const validateInput = e => {
    let { name, value } = e.target;
    setSignUpError(errors => {
      const stateObj = { ...errors, [name]: '' };
      switch (name) {
        case "password":
          if (!value)
            break;
          else if (value.length < 6)
            stateObj[name] = 'Minimum length is 6.';
          else if (signUpInput.confirmPassword && value !== signUpInput.confirmPassword)
            stateObj["confirmPassword"] = "The passwords do not match.";
          break;
        case "confirmPassword":
          if (!value) {
            break;
          } else if (signUpInput.password && value !== signUpInput.password) {
            stateObj[name] = "Password and Confirm Password does not match.";
          }
          break;
        default:
          break;
      }
      return stateObj;
    })
  }

  const navigate = useNavigate();

  const signup = async (e) => {
    if (signUpInput.password !== signUpInput.confirmPassword) {
      return (setsign('signup'));
    }
    e.preventDefault();
    // console.log(signUpInput);
    // console.log(typeof signUpInput.picture);
    const formData = new FormData();
    formData.append('email',signUpInput.email);
    formData.append('password',signUpInput.password);
    formData.append('confirmPassword',signUpInput.confirmPassword);
    formData.append('firstName',signUpInput.firstName);
    formData.append('lastName',signUpInput.lastName);
    formData.append('birthDate',signUpInput.birthDate);
    signUpInput.picture !== '' ? formData.append('picture',signUpInput.picture):formData.append('picture',null);
    
    const response = await axios.post("https://localhost:7210/api/Auth/register", formData,{
      headers: { "Content-Type": "multipart/form-data" },
    }).catch((error) => {
      console.log(JSON.stringify(error.response.data));
      setSignUpError({ ...setSignUpError, result: "Invalid data or user already exist." });
    });

    setSignUpInput({ ...setSignUpInput, key: "" });
    setSignUpError({ ...setSignUpError, key: "" });
    if (response?.data) {
      //console.log("Registration success");
      const token = encodeURIComponent(response.data)
      //console.log(response.data);
      const link = `/signup_success/${token}`;
      navigate(link);
    }
  };

  const signin = async (e) => {
    e.preventDefault();

    await axios.post("https://localhost:7210/api/Auth/login", {
      email: signInInput.email,
      password: signInInput.password
    }).then(response => {
      //console.log(response);
      if (response.data) {
        localStorage.setItem("token", response.data);
        navigate("/profile");
      }
    }

    );
  };

  useEffect(() =>{
    if(sign === 'signup'){
      document.querySelector('.login').style.height = "110vh";
    }else{
      document.querySelector('.login').style.height = "100vh";
    }
  }
  )

  return (
    <div className='login gradient_bg'>
      <div className='login_head'>
        <h1>Welcome to SocialApp</h1>
        <h2>Sign Up to Get Started</h2>
      </div>
      <div className='login_group'>
        {sign === 'signin' && <h2>Log In</h2>}
        {sign === 'signup' && <h2>Sign Up</h2>}

        <div className='login_group_btn'>
          <button onClick={() => setsign('signin')}>
            Log In
          </button>
          <hr />
          <button onClick={() => setsign('signup')}>
            Sign Up
          </button>
        </div>
      </div>
      {sign === 'signin' && (
        <div className='login_body'>
          <form className='login_form' onSubmit={(e) => signin(e)}>
            <label className="login_form-l">Email </label>
            <input name='email' type="email" className="login_form-i"
              value={signInInput.email}
              onChange={onSignInInputChange}
              placeholder="Email" required />

            <label className="login_form-l">Password </label>
            <input className="login_form-i" name='password' type="password"
              value={signInInput.password}
              onChange={onSignInInputChange}
              placeholder="Password" required />

            <button type="submit" className="login_form-btn">Sign In</button>
          </form>
        </div>
      )}

      {sign === 'signup' && (
        <div className='login_body'>
          <form className='login_form' onSubmit={(e) => { signup(e); e.target.reset(); }}>
            <label className="login_form-l">First Name </label>
            <input name='firstName' type="text" className="login_form-i"
              value={signUpInput.firstName}
              onChange={onSignUpInputChange}
              placeholder="First Name" required />

            <label className="login_form-l">Last Name </label>
            <input name='lastName' type="text" className="login_form-i"
              value={signUpInput.lastName}
              onChange={onSignUpInputChange}
              placeholder="Last Name" required />

            <label className="login_form-l">Birth Date </label>
            <input name='birthDate' type="date" className="login_form-i"
              value={signUpInput.birthDate}
              onChange={onSignUpInputChange} required/>

            <label className="login_form-l">Email </label>
            <input name='email' type="email" className="login_form-i"
              value={signUpInput.email}
              onChange={onSignUpInputChange}
              placeholder="Email" required />

            <label className="login_form-l">Password </label>
            <input name='password' className="login_form-i" type="password"
              value={signUpInput.password}
              onChange={onSignUpInputChange}
              onBlur={validateInput}
              placeholder="Password" required />
            <p>{signUpError.password}</p>

            <label className="login_form-l">Confirm Password </label>
            <input name='confirmPassword' className="login_form-i" type="password"
              value={signUpInput.confirmPassword}
              onChange={onSignUpInputChange}
              onBlur={validateInput}
              placeholder="Confirm Password" required />
            <p>{signUpError.confirmPassword}</p>

            <label className="login_form-l">Picture </label>
            {/* <input name='picture' type="file" value={signUpInput.picture} onChange={onSignUpInputChange} /> */}
            <Dropzone
              acceptedFiles=".jpg,.jpeg,.png"
              multiple={false}
              onDrop={(acceptedFiles) => {

                setSignUpInput(inputs => ({
                  ...inputs,
                  picture: acceptedFiles[0]
                }))
              }
              }>
              {({ getRootProps, getInputProps }) => (
                <div className='login_form_picture_box'{...getRootProps()}>
                  <input {...getInputProps()} />
                  {signUpInput.picture ? (
                    <div >
                      Selected file: {signUpInput.picture.name}
                    </div>
                  ) : (
                    <div>Add picture here</div>
                  )}
                </div>
              )}
            </Dropzone> 

            <button type="submit" className="login_form-btn">Register</button>
            <p>{signUpError.result}</p>
          </form>
        </div>
      )}
    </div>
  )
}

export default Login