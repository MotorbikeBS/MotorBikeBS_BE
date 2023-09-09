using AutoMapper;
using BusinessObject.Models;
using MotorBikeBS_API.DTO;

namespace MotorBikeBS_API.AutoMapper
{
	public class MappingConfig
	{
		public static MapperConfiguration RegisterMaps()
		{
			var mappingConfig = new MapperConfiguration(config =>
			{
				config.CreateMap<Motorbike, MotobikeCreateDTO>().ReverseMap();
			});
			return mappingConfig;
		}
	}
}
