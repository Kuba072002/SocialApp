import { React, useState } from 'react'
import { MdOutlineImage, MdSend } from "react-icons/md";
import Dropzone from 'react-dropzone';
import axios from 'axios';
import Account from '../../assets/account.png'
import "./addPost.css"
import apiconfig from "../../apiconfig.json"

const AddPost = ({ userPicture }) => {
    const [content, setContent] = useState('');
    const [dropzone, setDropzone] = useState(false);
    const [pictures, setPictures] = useState('');

    const addPost = async (e) => {
        console.log(pictures)
        e.preventDefault();
        const formData = new FormData();
        formData.append('content', content);
        if (pictures !== ''){
            pictures.forEach(element => {
                formData.append('pictures', element)
            });
        }else{
            formData.append('pictures', null);
        }
        const token = localStorage.getItem('token');
        const response = await axios.post(`${apiconfig.API_KEY}Post/AddPost`, formData, {
            headers: { 'Authorization': `Bearer ${token}`,"Content-Type": "multipart/form-data" },
        }).catch((error) => {
            console.log(JSON.stringify(error.response.data));
        });
        if(response.status === 200){
            console.log("Get posts again")
        }
        setContent('');
        setDropzone(false);
        setPictures('');
    };

    return (
        <div className='addpost bg'>
            <div className='addpost_container'>
                <div className='addpost_container-image'>
                    {userPicture ? (
                        <img src={`data:image/${userPicture.fileExtension};base64,${userPicture.data}`} alt={userPicture.name} />
                    ) : <img src={Account} alt={Account} />}
                </div>
                <textarea type="text" className="addpost_container-form-i"
                    value={content}
                    onChange={(e) => {
                        setContent(e.target.value);
                    }}
                    placeholder="What's on your mind" required />
            </div>
            {dropzone && (
                <Dropzone
                    acceptedFiles=".jpg,.jpeg,.png"
                    multiple={true}
                    onDrop={(acceptedFiles) => {
                        setPictures([...pictures, ...acceptedFiles]);
                    }
                    }>
                    {({ getRootProps, getInputProps }) => (
                        <div className='addpost_picture_box'{...getRootProps()}>
                            <input {...getInputProps()} />
                            {pictures ? (
                                <div >
                                    Selected file: {pictures[0].name}
                                </div>
                            ) : (
                                <div>Add picture here</div>
                            )}
                        </div>
                    )}
                </Dropzone>
            )}
            <hr className='addpost_hr' />
            <div className='addpost_buttons'>
                <button onClick={() => dropzone ? setDropzone(false) : setDropzone(true)}>
                    <MdOutlineImage color={"#fff"} size={27} />
                    Image
                </button>
                <button type="button" onClick={(e) => addPost(e)}>
                    Add
                    <MdSend color={"#fff"} size={24} />
                </button>
            </div>

        </div>
    )
}

export default AddPost