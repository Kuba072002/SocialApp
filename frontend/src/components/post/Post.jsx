import React, { useState } from 'react'
import Account from '../../assets/account.png'
import { Tooltip } from 'react-tooltip';
import { MdPersonRemove, MdOutlineFavoriteBorder, MdOutlineMessage } from "react-icons/md";
import './post.css'
import getTimeAgo from '../../utils/utils';

import Slider from "react-slick";
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";

const Post = ({ id, content, createDate, userId, firstName, lastName, userPicture, pictures, isProfile = false }) => {
  // const [currentPostIndex, setCurrentPostIndex] = useState(0);
  // const [currentPictureIndex, setCurrentPictureIndex] = useState(0);

  const [selectedImage, setSelectedImage] = useState(null);
  const handleImageClick = (imageSrc) => {
    setSelectedImage(imageSrc);
  };

  const handleCloseModal = () => {
    setSelectedImage(null);
  };
  // const handlePictureClick = () => {
  //   setCurrentPictureIndex((currentPictureIndex + 1) % pictures.length);
  // };

  var settings = {
    arrows: true,
    // lazyLoad: true,
    // fade: true,
    // centerMode: true,
    // centerPadding:true,
    infinite: true,
    speed: 300,
    slidesToShow: 1,
    slidesToScroll: 1,
    adaptiveHeight: true,
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
          {!isProfile &&
            <MdPersonRemove color="#fff" size={28} />}
        </div>
        <hr className='post_hr' />
        <div className='post_container-content'>
          <p>{content}</p>
        </div>
        <div className='post_container-pictures'>
          <Slider {...settings} >
            {pictures && pictures.map((picture, index) => (
              <div key={index}>
                <img src={`data:image/${picture.fileExtension};base64,${picture.data}`} alt={`${index + 1}`}
                  onDoubleClick={() => handleImageClick(picture)}
                />
              </div>
            ))}
          </Slider>
          {selectedImage && (
            <div className="modal" onClick={handleCloseModal}>
              <div className="modal-content">
                <img src={`data:image/${selectedImage.fileExtension};base64,${selectedImage.data}`} alt="full_size" />
                {/* <button onClick={handleCloseModal}>Close</button> */}
              </div>
            </div>
          )}
        </div>
        <hr className='addpost_hr' />
        <div className='post_container-reactions'>
          <MdOutlineFavoriteBorder color="#fff" size={24} />
          <MdOutlineMessage color="#fff" size={24} />
          {/* MdOutlineFavorite MdMessage*/}
        </div>
      </div>
    </div>
  )
}

export default Post