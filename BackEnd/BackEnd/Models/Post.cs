﻿namespace BackEnd.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string CreateDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Picture> Pictures { get; set; } 
        public List<Like> Likes { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
