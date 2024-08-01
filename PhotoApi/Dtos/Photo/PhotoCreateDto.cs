using System.ComponentModel.DataAnnotations;

namespace PhotoApi.Dtos.Photo
{
    public class PhotoCreateDto
    {
        public IFormFile File { get; set; }
        public string Description { get; set; }
    }
}

