﻿using System.ComponentModel.DataAnnotations;

namespace magic_villa_web.Models.Dto
{
	public class VillaDTO
	{

        public int Id { get; set; }

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
