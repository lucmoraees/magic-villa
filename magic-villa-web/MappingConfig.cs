using System;
using AutoMapper;
using magic_villa_web.Models;
using magic_villa_web.Models.Dto;

namespace magic_villa_web
{
	public class MappingConfig: Profile
	{
		public MappingConfig()
        {
            CreateMap<VillaDTO, VillaCreateDTO>().ReverseMap();
            CreateMap<VillaDTO, VillaUpdateDTO>().ReverseMap();

            CreateMap<VillaNumberDTO, VillaNumberCreateDTO>().ReverseMap();
            CreateMap<VillaNumberDTO, VillaNumberUpdateDTO>().ReverseMap();
        }
	}
}

