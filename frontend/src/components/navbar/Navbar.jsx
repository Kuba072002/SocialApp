import { React, useEffect, useState } from 'react'
import { Link, useNavigate } from "react-router-dom";
import axios from 'axios';
import apiconfig from "../../apiconfig.json";
import { ReactComponent as Logo } from '../../assets/logo.svg';
import {MdLogout} from "react-icons/md"; 
import "./navbar.css"

export let myUserId;

const Navbar = () => {
  const navigate = useNavigate();
  const [userId, setUserId] = useState('');//userid export const

  const logOut = async (e) => {
    localStorage.removeItem('token');
    navigate('/');
  }

  const fetchData = async () => {
    try {
      const token = localStorage.getItem('token');
      const response = await axios.get(`${apiconfig.API_KEY}User/GetMyId`, {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });
      setUserId(response.data);
      myUserId = response.data.toString();
    } catch (error) {
      console.log(error.response);
    }
  }

  useEffect(() => {
    if(userId === '')
      fetchData();
  },[userId]);
  const profileLink = `/profile/${userId}`

  return (
    <nav className='navbar bg'>
      <div className='navbar-links'>
        <div className='navbar-links_logo'>
          <Logo fill="#767a7c" />
        </div>
        <div className='navbar-links_container'>
          <p><Link to="/home">Home</Link></p>
          <p><Link to={profileLink}>Profile</Link></p>
          <p><Link to="/friends">Friends</Link></p>
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