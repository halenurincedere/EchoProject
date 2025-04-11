using System.ComponentModel.DataAnnotations;

namespace Echo.Business.Operations.Match.Dtos
{
    public class CreateMatchDto
    {
        [Required]
        public Guid SpeakerId { get; set; }

        [Required]
        public Guid ListenerId { get; set; }
    }
}