using System.ComponentModel.DataAnnotations;

namespace Bl.OpenAi.Sample.Image.Models
{
    public class ImageGenerationModel
    {
        [Required]
        public string? Prompt { get; set; }

        public string? RevisedPrompt { get; set; }
        public string? ImageUrl { get; set; }
    }
}
