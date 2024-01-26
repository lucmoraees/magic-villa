using System.ComponentModel.DataAnnotations;

namespace magic_villa_api.Models.Dto
{
	public class VillaNumberCreateDTO
    {
        [Required]
        public int VillaNo { get; set; }

        [Required]
        public int VillaId { get; set; }

        public string SpecialDetails { get; set; }
    }
}
