using API.DTO;
using AutoMapper;
using Core.Models;

namespace API.AutoMapper
{
	public class MappingConfig
	{
		public static MapperConfiguration RegisterMaps()
		{
			var mappingConfig = new MapperConfiguration(config =>
			{
				config.CreateMap<User, LoginDTO>().ReverseMap();
				config.CreateMap<User, RegisterDTO>().ReverseMap();
			});
			return mappingConfig;
		}
	}
}
