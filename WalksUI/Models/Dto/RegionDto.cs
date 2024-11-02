using System.ComponentModel.DataAnnotations;
namespace WalksUI.Models.Dto
{
    public class RegionDto
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Code is required.")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
    
}
