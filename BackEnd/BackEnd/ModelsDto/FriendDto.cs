﻿namespace BackEnd.ModelsDto
{
    public class FriendDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? AddedDate { get; set; }
        public PictureDto? Picture { get; set; }
    }
}
