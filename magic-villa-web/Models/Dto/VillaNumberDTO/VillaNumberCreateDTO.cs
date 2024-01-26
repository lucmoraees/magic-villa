using System.ComponentModel.DataAnnotations;

namespace magic_villa_web.Models.Dto
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
