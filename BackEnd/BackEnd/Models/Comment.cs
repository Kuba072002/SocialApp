﻿namespace BackEnd.Models
{
    public class Comment
    {
        public string Content { get; set; }
        public string CreateDate { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
