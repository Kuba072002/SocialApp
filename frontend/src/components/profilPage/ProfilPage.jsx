import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Navbar } from '../';
import InfoSection from './InfoSection';
import AddPost from '../addPost/AddPost';
import "./profilePage.css"

const ProfilPage = () => {
    const [userData, setUserData] = useState('');

    useEffect(() => {
        async function fetchData() {
            try {
                const token = localStorage.getItem('token');
                const response = await axios.get('https://localhost:7210/api/User/GetMe', {
                    headers: {
                        'Authorization': `Bearer ${token}`
                    }
                });
                setUserData(response.data);
            } catch (error) {
                console.log(error.response);
            }
        }
        fetchData();
    }, []);
    // const handleClick = async () => {
    // const token = localStorage.getItem('token');
    // const response = await axios.get("https://localhost:7210/api/User/GetMe",{headers:{'Authorization': `Bearer ${token}`}})
    //     .catch((error) => {console.log(JSON.stringify(error.response.data));});
    // setUserData(response.data);};

    return (
        <div>
            <Navbar />
            <div className='profilepage bg'>
                <div className='profilepage_info'>
                    <InfoSection data={userData} />
                </div>
                <div className='profilepage_post'>
                    <AddPost userPicture={userData.picture}/>
                </div>
            </div>
            {/* <h1>ProfilPage</h1>
                <button onClick={handleClick}>Get me</button>
                <br/>
                {userData.picture && (
                    <img src={`data:image/${userData.picture.fileExtension};base64,${userData.picture.data}`} alt={userData.picture.name} />
                )} */}
        </div>
    )
}

export default ProfilPage