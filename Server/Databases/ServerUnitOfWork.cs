using Server.Databases.Sql.Core;
using Server.Databases.Sql.Models;

namespace Server.Databases
{
    public interface IServerUnitOfWork : IUnitOfWork
    {
        IRepository<User> Users { get; }
        IRepository<Leaderboard> Leaderboards { get; }
    }

    public class ServerUnitOfWork : UnitOfWork, IServerUnitOfWork
    {
        public ServerUnitOfWork(ServerContext context)
            : base(context)
        { }

        private IRepository<User> users;
        public IRepository<User> Users => users ??= new Repository<ServerUnitOfWork, User>(this);

        private IRepository<Leaderboard> leaderboards;
        public IRepository<Leaderboard> Leaderboards => leaderboards ??= new Repository<ServerUnitOfWork, Leaderboard>(this);
    }
}
