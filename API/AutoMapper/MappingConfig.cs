using API.DTO.MotorbikeDTO;
using API.DTO.UserDTO;
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
                config.CreateMap<User, RegisterDTO>().ReverseMap().ForSourceMember(source => source.PasswordConfirmed, opt => opt.DoNotValidate());
                config.CreateMap<User, ResetPasswordDTO>().ReverseMap().ForSourceMember(source => source.PasswordConfirmed, opt => opt.DoNotValidate());
				config.CreateMap<User, LoginResponseDTO>().ReverseMap().ForSourceMember(source => source.Token, opt => opt.DoNotValidate());
				config.CreateMap<User, UserUpdateDTO>().ReverseMap();

				config.CreateMap<StoreDesciption, StoreRegisterDTO>().ReverseMap();

                config.CreateMap<Motorbike, MotorRegisterDTO>().ReverseMap();
                config.CreateMap<MotorbikeBrand, BrandRegisterDTO>().ReverseMap();
                config.CreateMap<MotorbikeModel, ModelRegisterDTO>().ReverseMap();
                config.CreateMap<MotorbikeBrand, BrandRegisterDTO>().ReverseMap();
                config.CreateMap<MotorbikeStatus, StatusRegisterDTO>().ReverseMap();
                config.CreateMap<MotorbikeType, TypeRegisterDTO>().ReverseMap();
                config.CreateMap<MotorbikeImage, ImageRegisterDTO>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
