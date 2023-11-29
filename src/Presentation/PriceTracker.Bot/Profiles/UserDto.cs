using AutoMapper;
using Telegram.Bot.Types;

namespace PriceTracker.Bot.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, Domain.Entities.User>();
    }
}