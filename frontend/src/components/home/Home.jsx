import React from 'react'
import "./home.css"
import { ReactComponent as Logo } from '../../assets/logo.svg';
import { AiFillGithub } from 'react-icons/ai';
import 'animate.css'
import { Link } from 'react-router-dom';

const Home = () => {
  return (
    <div className='home gradient_bg'>
      <div className='home_corner'>
        <a href="https://github.com/Kuba072002/SocialApp" target="_blank" rel="noreferrer">
          <AiFillGithub color="#fff" size={37}/>
        </a>
      </div>
      <div className='home_logo'>
        <Logo className="animate__animated animate__lightSpeedInRight" fill="#767a7c" />
        <p className="animate__animated animate__lightSpeedInRight">SOCIALAPP</p>
      </div>
      <div className='home_button'>
        <Link to="/login">
          <button className="animate__animated animate__lightSpeedInRight">GET STARTED</button>
        </Link>
      </div>
    </div>
  )
}


export default Home