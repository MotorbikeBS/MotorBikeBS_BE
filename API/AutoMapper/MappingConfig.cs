using API.DTO.MotorbikeDTO;
using API.DTO.Role;
using API.DTO.UserDTO;
using AutoMapper;
using Core.Models;
using Service.UnitOfWork;

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
                config.CreateMap<User, UserResponseDTO>().ReverseMap().ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));
				config.CreateMap<User, UserUpdateDTO>().ReverseMap();

				config.CreateMap<Role, RoleResponseDTO>().ReverseMap();

				config.CreateMap<StoreDesciption, StoreRegisterDTO>().ReverseMap();

                config.CreateMap<Motorbike, MotorRegisterDTO>().ReverseMap();
                config.CreateMap<MotorbikeBrand, BrandRegisterDTO>().ReverseMap();
                config.CreateMap<MotorbikeModel, ModelRegisterDTO>().ReverseMap();
                config.CreateMap<MotorbikeModel, ModelResponseDTO>().ReverseMap().ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand));
                
                config.CreateMap<MotorbikeStatus, StatusRegisterDTO>().ReverseMap();
                config.CreateMap<MotorbikeType, TypeRegisterDTO>().ReverseMap();
                config.CreateMap<MotorbikeImage, ImageRegisterDTO>().ReverseMap();

                config.CreateMap<Motorbike, MotorResponseDTO>().ReverseMap()
                    .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model))
                    .ForMember(dest => dest.MotorStatus, opt => opt.MapFrom(src => src.MotorStatus))
                    .ForMember(dest => dest.MotorType, opt => opt.MapFrom(src => src.MotorType))
                    .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner))
                    .ForMember(dest => dest.Store, opt => opt.MapFrom(src => src.Store))
                    .ForMember(dest => dest.MotorbikeImages, opt => opt.MapFrom(src => src.MotorbikeImages));
            });
            return mappingConfig;
        }
    }
}
