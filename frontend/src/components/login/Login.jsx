import { React, useState } from 'react'
import axios from 'axios';
import "./login.css"

const Login = () => {

  const [sign, setsign] = useState("signin");

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  
  const [errorPassword, setErrorPassword] = useState("");
  const [error, setError] = useState("");

  const signup = async (e) => {
    password.length < 6 ? setErrorPassword('Minimum length is 6.') : setErrorPassword('');
    password !== confirmPassword ? setError('The passwords do not match.') : setError('');

    e.preventDefault();
    //console.log('sent');

    const response = await axios.post("https://localhost:52094/api/Auth/register", {
      email: email,
      password: password,
      confirmPassword: confirmPassword
    });
    console.log(JSON.stringify(response?.data));
    e.target.reset();
  };

  const signin = async (e) => {
    e.preventDefault();

    await axios.post("https://localhost:52094/api/Auth/login",{
      email: email,
      password: password
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
            <input type="email" className="login_form-i" value={email} onChange={(e) => setEmail(e.target.value)} placeholder="Email" required />

            <label className="login_form-l">Password </label>
            <input className="login_form-i" type="password" value={password} onChange={(e) => setPassword(e.target.value)} placeholder="Password" required />

            <button type="submit" className="login_form-btn">Sign In</button>
          </form>
        </div>
      )}

      {sign === 'signup' && (
        <div className='login_body'>
          <form className='login_form' onSubmit={(e) => signup(e)}>
            <label className="login_form-l">Email </label>
            <input type="email" className="login_form-i" value={email} onChange={(e) => setEmail(e.target.value)} placeholder="Email" required />

            <label className="login_form-l">Password </label>
            <input className="login_form-i" type="password" value={password} onChange={(e) => setPassword(e.target.value)} placeholder="Password" required />
            <p>{errorPassword}</p>

            <label className="login_form-l">Confirm Password </label>
            <input className="login_form-i" type="password" value={confirmPassword} onChange={(e) => setConfirmPassword(e.target.value)} placeholder="Confirm Password" required />
            <p>{error}</p>

            <button type="submit" className="login_form-btn">Register</button>
          </form>
        </div>
      )}
    </div>



    // <div className="form">
    //   <div className="form-body">
    //     <div className="email">
    //       <label className="form__label" htmlFor="email">Email </label>
    //       <input type="email" className="form__input" value={email} onChange={(e) => setEmail(e.target.value)} placeholder="Email" required />
    //     </div>
    //     <div className="password">
    //       <label className="form__label" htmlFor="password">Password </label>
    //       <input className="form__input" type="password"  value={password} onChange={(e) => setPassword(e.target.value)} placeholder="Password" required/>
    //     </div>
    //     <div className="confirm-password">
    //       <label className="form__label" htmlFor="confirmPassword">Confirm Password </label>
    //       <input className="form__input" type="password" value={confirmPassword} onChange={(e) => setConfirmPassword(e.target.value)} placeholder="Confirm Password" required />
    //     </div>
    //   </div>
    //   <div className="footer">
    //     <button onClick={(e) => {signup(e)}} type="submit" className="btn">Register</button>
    //   </div>
    // </div>

  )
}

export default Login