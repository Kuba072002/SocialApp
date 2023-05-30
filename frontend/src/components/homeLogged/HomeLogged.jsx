import { React, useEffect, useState } from 'react'
import axios from 'axios';
import apiconfig from "../../apiconfig.json"
import { FriendWidget,InfoSection, AddPost, Post } from "../"
// import FriendWidget from '../friendWidget/FriendWidget';
import '../profilPage/profilePage.css'

const HomeLogged = () => {
  const [userData, setUserData] = useState('');

  const fetchData = async () => {
    try {
      const token = localStorage.getItem('token');
      const response = await axios.get(`${apiconfig.API_KEY}User/getHomeData`, {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });
      setUserData(response.data);
    } catch (error) {
      console.log(error.response);
    }
  }

  useEffect(() => {
    fetchData();
  },[]);

  return (
    <div>
      <div className='profilepage bg2'>
        <div className='profilepage_info'>
          <InfoSection data={userData} />
        </div>

        <div className='profilepage_post'>
          <AddPost userPicture={userData.picture} />
          {userData && userData.posts.map((p, i) =>
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
          {userData && userData.friends.map((f, i) =>
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

export default HomeLogged