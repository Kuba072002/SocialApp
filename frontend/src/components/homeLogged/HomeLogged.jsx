import { React, useEffect, useState } from 'react'
import axios from 'axios';
import apiconfig from "../../apiconfig.json"
import { InfoSection, AddPost, Post } from "../"

const HomeLogged = () => {
  const [user, setUser] = useState('');

  const fetchData = async () => {
    try {
      const token = localStorage.getItem('token');
      const response = await axios.get(`${apiconfig.API_KEY}User/getHomeData`, {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });
      setUser(response.data);
    } catch (error) {
      console.log(error.response);
    }
  }

  useEffect(() => {
    fetchData();
    // console.log(user)
  });

  return (
    <div>
            <div className='profilepage bg2'>
                <div className='profilepage_info'>
                    <InfoSection data={user} />
                </div>
                <div className='profilepage_post'>
                    <AddPost userPicture={user.picture} />
                    {user && user.posts.map((p,i) => 
                        <Post key={i} 
                            id={p.id}
                            content={p.content}
                            createDate={p.createDate}
                            userId={p.userId}
                            firstName={user.firstName}
                            lastName={user.lastName}
                            userPicture={user.picture}
                            pictures={p.pictures}
                        />
                    )}
                </div>
            </div>
        </div>
  )
}

export default HomeLogged