﻿using System.ComponentModel.DataAnnotations;

namespace magic_villa_api.Models.Dto
{
	public class VillaUpdateDTO
	{
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public string Details { get; set; }

        public double Rate { get; set; }

        [Required]
        public int SquareFeet { get; set; }

        [Required]
        public int Occupancy { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public string Amenity { get; set; }
    }
}
