using AutoMapper;
using Server.Databases.Sql.Models;

namespace Server.Services
{
    public class ServerProfile : Profile
    {
        public ServerProfile()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<Leaderboard, LeaderboardViewModel>();
        }
    }
}
