import React, { useState, useEffect } from 'react';
import { useParams } from "react-router-dom";
import axios from 'axios';
import InfoSection from './InfoSection';
import { AddPost, Post } from '../';
import "./profilePage.css"

const ProfilPage = () => {
    const [userData, setUserData] = useState('');
    // const [posts, setPosts] = useState([]);
    const { userId } = useParams();

    // async function fetchPosts() {
    //     try {
    //         const token = localStorage.getItem('token');
    //         const response = await axios.get(`https://localhost:7210/api/User/GetMyPosts/${userId}`, {
    //             headers: {
    //                 'Authorization': `Bearer ${token}`
    //             }
    //         });
    //         setPosts(response.data);
    //     } catch (error) {
    //         console.log(error.response);
    //     }
    // }
    const fetchData = async () => {
        try {
            const token = localStorage.getItem('token');
            const response = await axios.get(`https://localhost:7210/api/User/GetUser/${userId}`, {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            });
            // console.log(response.data);
            setUserData(response.data);
        } catch (error) {
            console.log(error.response);
        }
    }

    useEffect(() => {  
        fetchData();
    });
    
    return (
        <div>
            <div className='profilepage bg2'>
                <div className='profilepage_info'>
                    <InfoSection data={userData} />
                </div>
                <div className='profilepage_post'>
                    <AddPost userPicture={userData.picture} />
                    {userData && userData.posts.map((p,i) => 
                        <Post key={i} 
                            id={p.id}
                            content={p.content}
                            createDate={p.createDate}
                            userId={p.userId}
                            firstName={userData.firstName}
                            lastName={userData.lastName}
                            userPicture={userData.picture}
                            pictures={p.pictures}
                        />
                    )}
                </div>
            </div>
        </div>
    )
}

export default ProfilPage