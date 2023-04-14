using System.ComponentModel.DataAnnotations;

namespace BackEnd.ModelsDto
{
    public class AddPostDto
    {
        [Required]
        public string Content { get; set; } = string.Empty;
        public List<IFormFile>? Pictures { get; set; } = null;
    }
}
