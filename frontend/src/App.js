import React from 'react'
import { Home, Login ,Signup_success } from "./components"
import {
  BrowserRouter,
  Route,
  Routes
} from "react-router-dom";
import './App.css'


const App = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="login" element={<Login/>} />
        <Route path="/*" element={<Home />} />
        <Route path="/signup_success/:token" element={<Signup_success/>} />
      </Routes>
    </BrowserRouter>
  )
}

export default App