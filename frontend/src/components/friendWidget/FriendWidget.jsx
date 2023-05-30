import React from 'react';
import Account from '../../assets/account.png'
import "./friendwidget.css"
import getTimeAgo from '../../utils/utils';
import { Tooltip } from 'react-tooltip';
import { useNavigate } from 'react-router-dom';

const FriendWidget = ({ userId, firstName, lastName, addedDate, userPicture }) => {
    const navigate = useNavigate();
    const handleclick = () => {
        navigate(`/profile/${userId}`);
    };

    return (
        <div className='friendwidget bg'>
            <div className='friendwidget_container' onClick={handleclick}>
                <div className='friendwidget_container-image'>
                    {userPicture ? (
                        <img src={`data:image/${userPicture.fileExtension};base64,${userPicture.data}`} alt={userPicture.name} />
                    ) : <img src={Account} alt={Account} />}
                </div>
                <div className='friendwidget_container-name'>
                    <h5>{firstName} {lastName}</h5>
                    {/* <p>{getTimeAgo(addedDate)}</p> */}
                    <p data-tooltip-id="my-tooltip2" >{getTimeAgo(addedDate)}</p>
                    <Tooltip id="my-tooltip2" content={addedDate} place='right' />
                </div>
            </div>
        </div>
    )
}

export default FriendWidget