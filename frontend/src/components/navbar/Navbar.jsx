import React from 'react'
import { Link } from "react-router-dom";
import { ReactComponent as Logo } from '../../assets/logo.svg';
import "./navbar.css"

const Navbar = () => {
  return (
    <nav className='navbar gradient_bg'>
      <div className='navbar-links'>
        <div className='navbar-links_logo'>
          <Logo  fill="#767a7c" />
        </div>
        <div className='navbar-links_container'>
          <p><Link to="/">Home</Link></p>
          <p><Link to="/profile">Profile</Link></p>
        </div>
      </div>
    </nav>
  )
}

export default Navbar