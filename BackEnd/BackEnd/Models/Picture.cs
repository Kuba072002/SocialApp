using Azure.Core;
using Microsoft.AspNetCore.Http;
namespace BackEnd.Models
{
    public class Picture
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
        public string FileExtension { get; set; }
        public decimal Size { get; set; }
        public User User { get; set; }

        public void Create(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                this.Name = file.FileName;
                this.Data = Convert.ToBase64String(memoryStream.ToArray());
                this.FileExtension = Path.GetExtension(file.FileName);
                this.Size = file.Length;
                
            }
        }
    }
}
