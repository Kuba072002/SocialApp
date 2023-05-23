import React, { useState, useEffect } from 'react';
// import axios from 'axios';
import { MdOutlineWorkOutline, MdOutlineCake, MdOutlineLocationOn, MdOutlineAccountCircle } from "react-icons/md";
import Account from '../../assets/account.png'
import './infosection.css'

const InfoSection = ({ data }) => {
    const [userData, setUserData] = useState('');

    useEffect(() => {
        setUserData(data);
    }, [data]);

    function calculateAge(birthDate) {
        const currentDate = new Date();
        const birthYear = new Date(birthDate).getFullYear();
        const currentYear = currentDate.getFullYear();
        const age = currentYear - birthYear;
        return age;
    }
    // MdOutlineManageAccounts
    return (
        <div className='infosection bg'>
            <div className='infosection_container'>
                <div className='infosection_container-image'>
                    {userData.picture ? (
                        <img src={`data:image/${userData.picture.fileExtension};base64,${userData.picture.data}`} alt={userData.picture.name} />
                    ) : <img src={Account} alt={Account} />}
                </div>
                <div className='infosection_container-name'>
                    <p>{userData.firstName}</p>
                    <p>{userData.lastName}</p>
                </div>
                <div className='infosection_container-data'>
                    <div className='infosection_container-data-item'>
                        {/* <label>Birth date:</label>     */}
                        <MdOutlineCake color="#fff" size={28} />
                        {/* <RiCloseLine color="#fff" size={27} onClick={() => setToggleMenu(false) */}
                        <p>{`${userData.birthDate}`}</p>
                    </div>
                    <div className='infosection_container-data-item'>
                        {/* <label>Age:</label>     */}
                        <MdOutlineAccountCircle color="#fff" size={28} />
                        <p>{calculateAge(userData.birthDate).toString()}</p>
                    </div>
                    <hr className='infosection_hr' />
                    <div className='infosection_container-data-item'>
                        {/* <label>Location:</label>     */}
                        <MdOutlineLocationOn color="#fff" size={28} />
                        <p>{`${userData.location}`}</p>
                    </div>
                    <div className='infosection_container-data-item'>
                        {/* <label>Occupation:</label>     */}
                        <MdOutlineWorkOutline color="#fff" size={28} />
                        <p>{`${userData.occupation}`}</p>
                    </div>
                    {/* {userData.lastName}
                    {userData.birthDate} */}
                </div>

            </div>
        </div>
    )
}

export default InfoSection