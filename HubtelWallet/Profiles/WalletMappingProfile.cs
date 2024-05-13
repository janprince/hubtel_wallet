using AutoMapper;
using HubtelWallet.Dto;
using HubtelWallet.Models;

namespace HubtelWallet.Mappings
{
    public class WalletMappingProfile : Profile
    {
        public WalletMappingProfile()
        {
            CreateMap<WalletPostDto, Wallet>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.AccountNumber))
                .ForMember(dest => dest.Scheme, opt => opt.MapFrom(src => src.Scheme))
                .ForMember(dest => dest.OwnerPhoneNumber, opt => opt.MapFrom(src => src.OwnerPhoneNumber));

            CreateMap<Wallet, WalletResponseDto>();
        }
    }
}