using magic_villa_api.Models.Dto;

namespace magic_villa_api.Data
{
	public static class VillaStore
	{
        public static List<VillaDTO> villaList = new List<VillaDTO> {
                new VillaDTO{ Id = 1, Name = "Pool View", SquareFeet = 100, Occupancy = 4 },
                new VillaDTO{ Id = 2, Name = "Beach View", SquareFeet = 300, Occupancy = 3 }
            };
    }
}

