using System;
using AutoMapper;
using magic_villa_api.Models;
using magic_villa_api.Models.Dto;

namespace magic_villa_api
{
	public class MappingConfig: Profile
	{
		public MappingConfig()
        {
            CreateMap<Villa, VillaDTO>();
            CreateMap<VillaDTO, Villa>();

            CreateMap<Villa, VillaCreateDTO>().ReverseMap();
            CreateMap<Villa, VillaUpdateDTO>().ReverseMap();

            CreateMap<VillaNumber, VillaNumberDTO>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberCreateDTO>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberUpdateDTO>().ReverseMap();

            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
        }
	}
}

