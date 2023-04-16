import { Navigate, Outlet } from "react-router-dom";
import Navbar from "./navbar/Navbar";

export const ProtectedRoute = () => {
    const isAuthenticated = !!localStorage.getItem('token');
    if (!isAuthenticated) {
        return <Navigate to="/login" />;
    }
    return (
        <div>
            <Navbar />
            <Outlet />
        </div>
    );
};