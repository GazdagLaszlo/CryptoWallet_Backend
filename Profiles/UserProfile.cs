using AutoMapper;
using CryptoWallet.Dto;
using CryptoWallet.Model;

namespace CryptoWallet.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<UserCreateDTO, User>();
            CreateMap<UserPutDTO, User>();
            CreateMap<User, UserResponseDTO>();

            CreateMap<Wallet, WalletResponseDTO>();
            CreateMap<WalletBalancePutDTO, Wallet>();

            CreateMap<PortfolioItem, PortfolioItemResponseDTO>()
                .ForMember(dest => dest.CryptoName, opt => opt.MapFrom(src => src.Cryptocurrency.Name));
            CreateMap<PortfolioItemCreateDto, PortfolioItem>();
            //Nem kellett, azt írta hogy Cryptocurrency already being tracked.
            /*
                .ForMember(dest => dest.Wallet, opt => opt.MapFrom(src => new Wallet { Id = src.WalletId }))
                .ForMember(dest => dest.Cryptocurrency, opt => opt.MapFrom(src => new Cryptocurrency { Id = src.CryptocurrencyId }));
            */

            CreateMap<Cryptocurrency, CryptocurrencyShortResponseDTO>();

            CreateMap<TransactionCreateDTO, Transaction>();
            CreateMap<Transaction, TransactionShortResponseDTO>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type == 0 ? TransactionType.Buy.ToString() : TransactionType.Sell.ToString()));
            CreateMap<Transaction, TransactionResponseDTO>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type == 0 ? TransactionType.Buy.ToString() : TransactionType.Sell.ToString()))
                .ForMember(dest => dest.TotalExpense, opt => opt.MapFrom(src => src.Price * src.Amount));

            CreateMap<PriceHistory, PriceHistoryResponseDTO>();

            CreateMap<LimitOrderCreateDTO, LimitOrder>();
            CreateMap<LimitOrder, LimitOrderResponseDTO>()
                .ForMember(dest => dest.OrderType, opt => opt.MapFrom(src => src.OrderType == 0 ? OrderType.Buy.ToString() : OrderType.Sell.ToString()));
        }
    }
}
