import { React, useState } from 'react'
import axios from 'axios';
import { useNavigate } from "react-router-dom";
import "./login.css"

const Login = () => {

  const [sign, setsign] = useState("signin");

  const [signUpInput, setSignUpInput] = useState({
    email: '',
    password: '',
    confirmPassword:''
  });

  const [signUpError, setSignUpError] = useState({
    password: '',
    confirmPassword: ''
  });

  const [signInInput, setSignInInput] = useState({
    email: '',
    password: ''
  });

  const onSignUpInputChange = e => {
    const { name, value} = e.target;
    setSignUpInput( inputs => ({
      ...inputs,
      [name]: value
    }));
    if(name !== 'email')
      validateInput(e);
  };

  const onSignInInputChange = e => {
    const { name, value} = e.target;
    setSignInInput( inputs => ({
      ...inputs,
      [name]: value
    }));
  };

  const validateInput = e => {
    let { name, value} = e.target;
    setSignUpError( errors => {
      const stateObj = {...errors,[name]: ''};
      switch(name) {
        case "password":
          if(!value)
            break;
          else if(value.length < 6)
            stateObj[name] = 'Minimum length is 6.';
          else if(signUpInput.confirmPassword && value !== signUpInput.confirmPassword)
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
          console.log("asd");
          break;
      }
      return stateObj;
    })
  }
  
   //const [email, setEmail] = useState("");
   //const [password, setPassword] = useState("");
   //const [confirmPassword, setConfirmPassword] = useState("");
  
   //const [errorPassword, setErrorPassword] = useState("");
   //const [error, setError] = useState("");

  const navigate = useNavigate();  

  const signup = async (e) => {
    //password.length < 6 ? setErrorPassword('Minimum length is 6.') : setErrorPassword('');
    //password !== confirmPassword ? setError('The passwords do not match.') : setError('');
    
    // if(error.valueOf !== "" || errorPassword.valueOf !== "")
    //   alert("Cannot register")
    
    e.preventDefault();
    const response = await axios.post("https://localhost:7210/api/Auth/register", {
      email: signUpInput.email,
      password: signUpInput.password,
      confirmPassword: signUpInput.confirmPassword
    }).catch((error) => {
      console.log(JSON.stringify(error.response.data));
      //alert(JSON.stringify(error.response.data));
    });
    //console.log(JSON.stringify(response?.data));
    //setEmail("");
    //setPassword("");
    //setConfirmPassword("");
    for (const key in signInInput) {
      setSignUpInput[key] = '';
    }
    for (const key in signUpError) {
      setSignUpError[key] = '';
    }
    if(response?.data){
      console.log("Registration success")
    }
  };

  const signin = async (e) => {
    e.preventDefault();

    await axios.post("https://localhost:7210/api/Auth/login",{
      email: signInInput.email,
      password: signInInput.password
    }).then(response =>{
      console.log(response);
      if(response.data){
        console.log("Tak");
        localStorage.setItem("user", JSON.stringify(response.data));
      }
    }

    );
  };

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
          <button className="newbuttn" onClick={() => setsign('signin')}>
            Log In
          </button>
          <hr />
          <button className="newbuttn" onClick={() => setsign('signup')}>
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
            //value={email} 
            //onChange={(e) => setEmail(e.target.value)} 
            placeholder="Email" required />

            <label className="login_form-l">Password </label>
            <input className="login_form-i" name='password' type="password" 
            value={signInInput.password}
            onChange={onSignInInputChange}
            //value={password} 
            //onChange={(e) => setPassword(e.target.value)} 
            placeholder="Password" required />

            <button type="submit" className="login_form-btn">Sign In</button>
          </form>
        </div>
      )}

      {sign === 'signup' && (
        <div className='login_body'>
          <form className='login_form' onSubmit={(e) => {signup(e); e.target.reset();}}>
            <label className="login_form-l">Email </label>
            <input name='email' type="email" className="login_form-i" 
            value={signUpInput.email}
            onChange={onSignUpInputChange}
            //value={email} 
            //onChange={(e) => setEmail(e.target.value)} 
            placeholder="Email" required />

            <label className="login_form-l">Password </label>
            <input name='password' className="login_form-i" type="password" 
              value={signUpInput.password}
              onChange={onSignUpInputChange}
              onBlur={validateInput}
              //value={password} 
              //onChange={(e) => {setPassword(e.target.value); e.target.value.length < 6 ? setErrorPassword('Minimum length is 6.') : setErrorPassword('');}} 
              placeholder="Password" required />
            <p>{signUpError.password}</p>

            <label className="login_form-l">Confirm Password </label>
            <input name='confirmPassword' className="login_form-i" type="password" 
              value={signUpInput.confirmPassword}
              onChange={onSignUpInputChange}
              onBlur={validateInput}
            //value={confirmPassword} 
              //onChange={(e) => {setConfirmPassword(e.target.value);const {name,value} = e.target;console.log(name);console.log(value); e.target.value !== password ? setError('The passwords do not match.') : setError(''); }} 
              placeholder="Confirm Password" required />
            <p>{signUpError.confirmPassword}</p>

            <button type="submit" className="login_form-btn">Register</button>
          </form>
        </div>
      )}
    </div>
  )
}

export default Login