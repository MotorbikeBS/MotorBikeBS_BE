using API.DTO.BookingDTO;
using API.DTO.BookingNegotiationDTO;
using API.DTO.BuyerBookingDTO;
using API.DTO.ContractDTO;
using API.DTO.MotorbikeDTO;
using API.DTO.NegotiationDTO;
using API.DTO.OwnerDTO;
using API.DTO.RequestDTO;
using API.DTO.Role;
using API.DTO.StoreDTO;
using API.DTO.UserDTO;
using API.DTO.WishlistDTO;
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
				config.CreateMap<User, ChangePasswordDTO>().ReverseMap().ForSourceMember(source => source.PasswordConfirmed, opt => opt.DoNotValidate())
																		.ForSourceMember(source => source.OldPassword, opt => opt.DoNotValidate());
				config.CreateMap<User, LoginResponseDTO>().ReverseMap().ForSourceMember(source => source.Token, opt => opt.DoNotValidate());
				config.CreateMap<User, UserResponseDTO>().ReverseMap()
					.ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
					.ForMember(dest => dest.StoreDesciptions, opt => opt.MapFrom(src => src.StoreDesciptions));
				config.CreateMap<User, UserUpdateDTO>().ReverseMap();
				config.CreateMap<User, OwnerRegisterDTO>().ReverseMap();

				config.CreateMap<Role, RoleResponseDTO>().ReverseMap();
				config.CreateMap<StoreDesciption, StoreDescriptionResponseDTO>().ReverseMap();

				config.CreateMap<Negotiation, NegotiationCreateDTO>().ReverseMap();
				config.CreateMap<Negotiation, NegotiationResponseDTO>().ReverseMap();

				config.CreateMap<Booking, BookingCreateDTO>().ReverseMap();
				config.CreateMap<BuyerBooking, BookingResponseDTO>().ReverseMap();

				config.CreateMap<BuyerBooking, BuyerBookingCreateDTO>().ReverseMap();

				config.CreateMap<Contract, ContractCreateDTO>().ReverseMap();
				config.CreateMap<Contract, ContractResponseDTO>().ReverseMap();

				config.CreateMap<ContractImage, ContractImageResponseDTO>().ReverseMap();

				//Booking for Negotiation
				config.CreateMap<Booking, BookingNegoResponseDTO>().ReverseMap();
				config.CreateMap<Request, BookingNegoRequestResponseDTO>().ReverseMap().ForMember(dest => dest.Negotiations, opt => opt.MapFrom(src => src.Negotiations))
																					   .ForMember(dest => dest.Sender, opt => opt.MapFrom(src => src.Sender))
																					   .ForMember(dest => dest.Receiver, opt => opt.MapFrom(src => src.Receiver));
				
				//End

				config.CreateMap<StoreDesciption, StoreRegisterDTO>().ReverseMap().ForSourceMember(source => source.File, opt => opt.DoNotValidate())
																				  .ForSourceMember(source => source.License, opt => opt.DoNotValidate());

				config.CreateMap<Wishlist, WishlistResponseDTO>().ReverseMap();

				config.CreateMap<Motorbike, MotorRegisterDTO>().ReverseMap();
				config.CreateMap<Motorbike, MotorUpdateDTO>().ReverseMap();
				config.CreateMap<MotorRegisterDTO, MotorUpdateDTO>().ReverseMap();
				config.CreateMap<Motorbike, MotorResponseDTO>().ReverseMap()
					.ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model))
					.ForMember(dest => dest.MotorStatus, opt => opt.MapFrom(src => src.MotorStatus))
					.ForMember(dest => dest.MotorType, opt => opt.MapFrom(src => src.MotorType))
					.ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner))
					.ForMember(dest => dest.Store, opt => opt.MapFrom(src => src.Store))
					.ForMember(dest => dest.MotorbikeImages, opt => opt.MapFrom(src => src.MotorbikeImages));


				config.CreateMap<MotorbikeBrand, BrandRegisterDTO>().ReverseMap();
				config.CreateMap<MotorbikeModel, ModelRegisterDTO>().ReverseMap();
				config.CreateMap<MotorbikeModel, ModelResponseDTO>().ReverseMap().ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand));
				config.CreateMap<MotorbikeStatus, StatusRegisterDTO>().ReverseMap();
				config.CreateMap<MotorbikeStatus, StatusResponseDTO>().ReverseMap();
				config.CreateMap<MotorbikeType, TypeRegisterDTO>().ReverseMap();
				config.CreateMap<MotorbikeType, TypeResponseDTO>().ReverseMap();
				config.CreateMap<MotorbikeImage, ImageRegisterDTO>().ReverseMap();
				config.CreateMap<MotorbikeImage, ImageResponseDTO>().ReverseMap();

				config.CreateMap<RequestType, Type_RequestRegisterDTO>().ReverseMap();
				config.CreateMap<Request, BookingResponseRequestDTO>().ReverseMap().ForMember(dest => dest.Motor, opt => opt.MapFrom(src => src.Motor))
																				   .ForSourceMember(source => source.Sender, opt => opt.DoNotValidate());

				config.CreateMap<Request, NegotiationResponseRequestDTO>().ReverseMap().ForMember(dest => dest.Motor, opt => opt.MapFrom(src => src.Motor))
																					   .ForMember(dest => dest.Negotiations, opt => opt.MapFrom(src => src.Negotiations))
																					   .ForMember(dest => dest.Sender, opt => opt.MapFrom(src => src.Sender))
																					   .ForMember(source => source.Receiver, opt => opt.MapFrom(src => src.Receiver));


				config.CreateMap<Request, RequestRegisterDTO>().ReverseMap();
				config.CreateMap<Request, RequestResponseDTO>().ReverseMap()
					.ForMember(dest => dest.Receiver, opt => opt.MapFrom(src => src.Receiver))
					.ForMember(dest => dest.Sender, opt => opt.MapFrom(src => src.Sender))
					.ForMember(dest => dest.RequestType, opt => opt.MapFrom(src => src.RequestType));

			});
			return mappingConfig;
		}
	}
}
