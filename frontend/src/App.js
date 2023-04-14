import React from 'react'
import { Home, Login ,SignupSuccess, ProfilPage, HomeLogged } from "./components"
import {
  BrowserRouter,
  Route,
  Routes,
  Navigate 
} from "react-router-dom";
import './App.css'

const App = () => {
  const isTokenAvailable = localStorage.getItem('token');

  const homePage = isTokenAvailable ?<HomeLogged/> : <Home /> 

  return (
    <BrowserRouter>
      <Routes>
        <Route path="login" element={<Login/>} />
        <Route path="/*" element={homePage} />
        <Route path="/signup_success/:token" element={<SignupSuccess/>} />
        {isTokenAvailable ? (
          <Route path="/profile" element={<ProfilPage />} />
        ) : ( 
          <Navigate to="/login" />
        )}
      </Routes>
    </BrowserRouter>
  )
}

export default App