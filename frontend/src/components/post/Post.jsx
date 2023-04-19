import React, { useState } from 'react'
import Account from '../../assets/account.png'
import { Tooltip } from 'react-tooltip';
import { MdPersonRemove,MdOutlineFavoriteBorder,MdOutlineMessage } from "react-icons/md";
import './post.css'

const Post = ({ id, content, createDate, userId, firstName, lastName, userPicture, pictures }) => {
  // const [currentPostIndex, setCurrentPostIndex] = useState(0);
  const [currentPictureIndex, setCurrentPictureIndex] = useState(0);

  const handlePictureClick = () => {
    setCurrentPictureIndex((currentPictureIndex + 1) % pictures.length);
  };

  const getTimeAgo = (createDate) => {
    const dateObj = typeof createDate === 'string' ? new Date(createDate) : createDate;
    const now = new Date();
    const diff = now - dateObj;
    const minutes = Math.floor(diff / (1000 * 60));
    const hours = Math.floor(diff / (1000 * 60 * 60));
    const days = Math.floor(diff / (1000 * 60 * 60 * 24));
    const months = Math.floor(diff / (30 * 1000 * 60 * 60 * 24));

    if (minutes < 60) {
      return `${minutes} minutes ago`;
    } else if (hours < 24) {
      return `${hours} hours ago`;
    } else if (days < 30) {
      return `${days} days ago`;
    } else if (months < 12) {
      return `${months} months ago`;
    }
    else {
      return 'More than a year ago';
    }
  };

  return (
    <div key={id} className='post bg'>
      <div className='post_container'>
        <div className='post_container-user'>
          <div className='post_container-user-elem'>
            <div>
              {userPicture ? (
                <img src={`data:image/${userPicture.fileExtension};base64,${userPicture.data}`} alt={userPicture.name} />
              ) : <img src={Account} alt={Account} />}
            </div>
            <div>
              <h5>{firstName} {lastName}</h5>
              <p data-tooltip-id="my-tooltip" data-tooltip-content={createDate}>{getTimeAgo(createDate)}</p>
              <Tooltip id="my-tooltip" place='bottom' />
            </div>
          </div>
          <MdPersonRemove color="#fff" size={28} />
        </div>
        <hr className='post_hr' />
        <div className='post_container-content'>
          <p>{content}</p>
        </div>
        <div className='post_container-pictures'>
          {/* {post.pictures && post.pictures.map((picture, index) => (
                    <img key={index} src={`data:image/${picture.fileExtension};base64,${picture.data}`} alt={`${index + 1}`} />
                ))} */}
          {pictures && pictures.length > 0 && (
            <img
              src={`data:image/${pictures[currentPictureIndex].fileExtension};base64,${pictures[currentPictureIndex].data}`}
              alt={`${currentPictureIndex + 1} of ${pictures.length}`}
              onClick={handlePictureClick}
            />
          )}
        </div>
        <hr className='addpost_hr' />
        <div className='post_container-reactions'>
            <MdOutlineFavoriteBorder color="#fff" size={24}/>
            <MdOutlineMessage color="#fff" size={24}/>
            {/* MdOutlineFavorite MdMessage*/}
        </div>
      </div>
    </div>
  )
}

export default Post