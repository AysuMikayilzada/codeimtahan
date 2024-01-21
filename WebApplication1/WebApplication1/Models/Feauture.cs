using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Feauture
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
