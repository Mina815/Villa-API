using AutoMapper;
using Villa_API.Models;
using Villa_API.Models.Dto;

namespace Villa_API
{
	public class MappingConfig: Profile
	{
		public MappingConfig()
		{
			CreateMap<Villa, VillaDTO>().ReverseMap();
			CreateMap<VillaCreateDTO, VillaDTO>().ReverseMap();
			CreateMap<VillaUpdateDTO, VillaDTO>().ReverseMap();

		}
	}
}
