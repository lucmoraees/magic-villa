using System.ComponentModel.DataAnnotations;

namespace magic_villa_api.Models.Dto
{
	public class VillaCreateDTO
	{

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public string Details { get; set; }

        public double Rate { get; set; }

        public int SquareFeet { get; set; }

        public int Occupancy { get; set; }

        public string ImageUrl { get; set; }

        public string Amenity { get; set; }
    }
}
