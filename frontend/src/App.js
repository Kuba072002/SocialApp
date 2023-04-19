import React from 'react'
import { Home, Login ,SignupSuccess, ProfilPage, HomeLogged } from "./components"
import { ProtectedRoute } from './components/ProtectedRoute';
import {
  BrowserRouter,
  Route,
  Routes,
} from "react-router-dom";
import './App.css'

const App = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="login" element={<Login />} />
        <Route path="/*" element={<Home />} />
        <Route path="/signup_success/:token" element={<SignupSuccess/>} />
        
        <Route element={<ProtectedRoute />}>
          <Route path="home" element={<HomeLogged />} />
          <Route path="profile/:userId" element={<ProfilPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  )
}

export default App