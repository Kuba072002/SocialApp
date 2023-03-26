import React from 'react'
import { Home, Login } from "./components"
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
      </Routes>
    </BrowserRouter>
  )
}

export default App