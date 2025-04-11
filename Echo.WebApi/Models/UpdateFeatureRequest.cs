using System.ComponentModel.DataAnnotations;

namespace Echo.WebApi.Models
{
    public class UpdateFeatureRequest
    {
        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; } = string.Empty;

        [MaxLength(300, ErrorMessage = "Note cannot exceed 300 characters.")]
        public string? Note { get; set; } = string.Empty;

        [MaxLength(300, ErrorMessage = "Source cannot exceed 300 characters.")]
        public string? Source { get; set; } = string.Empty;

        [MaxLength(100, ErrorMessage = "Tag cannot exceed 100 characters.")]
        public string? Tag { get; set; } = string.Empty;
    }
}