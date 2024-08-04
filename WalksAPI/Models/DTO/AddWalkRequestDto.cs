using System.ComponentModel.DataAnnotations;
using WalksAPI.Models.Domain;

namespace WalksAPI.Models.DTO
{
    public class AddWalkRequestDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name has to be a maximum of 100 characters ")]
        public string Name { get; set; }
        [Required]
        [MaxLength(1000, ErrorMessage = "Description has to be a maximum of 1000 characters ")]
        public string Description { get; set; }
        [Required]
        [Range(0,50, ErrorMessage = "LengthInKm has to be a range of 0 to 50 ")]
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        public Guid RegionId { get; set; }
        public Guid DifficultyId { get; set; }

    }
}
