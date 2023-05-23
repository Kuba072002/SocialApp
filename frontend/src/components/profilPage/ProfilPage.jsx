import React, { useState, useEffect, useCallback } from 'react';
import { useParams } from "react-router-dom";
import axios from 'axios';
import InfoSection from './InfoSection';
import { AddPost, Post, FriendWidget } from '../';
import apiconfig from "../../apiconfig.json"
import "./profilePage.css";

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
    const fetchData = useCallback(async () => {
        try {
            const token = localStorage.getItem('token');
            const response = await axios.get(`${apiconfig.API_KEY}User/GetUserProfile/${userId}`, {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            });
            // console.log(response.data);
            setUserData(response.data);
        } catch (error) {
            console.log(error.response);
        }
    },[userId]);

    useEffect(() => {  
        fetchData();
    },[fetchData]);
    
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
                            isProfile={true}
                        />
                    )}
                </div>
                <div className='profilepage_friends'>
                    {userData && userData.friends.map((f,i) =>
                    <FriendWidget key={i} 
                    userId={f.id}
                    firstName={f.firstName}
                    lastName={f.lastName}
                    addedDate={f.addedDate}
                    userPicture={f.picture} />
                    )}
                </div>
            </div>
        </div>
    )
}

export default ProfilPage