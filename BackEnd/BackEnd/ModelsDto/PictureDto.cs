using BackEnd.Models;

namespace BackEnd.ModelsDto
{
    public class PictureDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
        public string FileExtension { get; set; }

        public PictureDto(int id, string name,string data,string fileExtension)
        {
            Id = id; Name = name; Data = data; FileExtension = fileExtension;
        }
    }
}
