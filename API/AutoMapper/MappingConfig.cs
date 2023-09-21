﻿using API.DTO.UserDTO;
using API.DTO.MotorbikeDTO;
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
				config.CreateMap<User, DTO.UserDTO.RegisterDTO>().ReverseMap().ForSourceMember(source => source.PasswordConfirmed, opt => opt.DoNotValidate());
				config.CreateMap<User, ResetPasswordDTO>().ReverseMap().ForSourceMember(source => source.PasswordConfirmed, opt => opt.DoNotValidate()); ;
				config.CreateMap<StoreDesciption, StoreRegisterDTO>().ReverseMap();
                config.CreateMap<Motorbike, DTO.MotorbikeDTO.RegisterDTO> ().ReverseMap();
            });
			return mappingConfig;
		}
	}
}
