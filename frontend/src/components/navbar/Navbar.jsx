import React from 'react'
import { Link, useNavigate } from "react-router-dom";
// import jwtDecode from 'jsonwebtoken';
import { ReactComponent as Logo } from '../../assets/logo.svg';
import {MdLogout} from "react-icons/md"; 
import "./navbar.css"

const Navbar = () => {
  const navigate = useNavigate();

  const logOut = async (e) => {
    localStorage.removeItem('token');
    navigate('/');
  }

  return (
    <nav className='navbar bg'>
      <div className='navbar-links'>
        <div className='navbar-links_logo'>
          <Logo fill="#767a7c" />
        </div>
        <div className='navbar-links_container'>
          <p><Link to="/home">Home</Link></p>
          <p><Link to="/profile">Profile</Link></p>
        </div>
      </div>
      <div className='navbar_account'>
        <button onClick={(e) => logOut(e)}>
          Sign out
          <MdLogout color={"#fff"} size={24} />
        </button>
      </div>
    </nav>
  )
}

export default Navbar