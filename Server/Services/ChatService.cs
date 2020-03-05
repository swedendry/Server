using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Server.Extensions;
using Server.Hubs;
using Server.Hubs.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface IChatService
    {
        Task Login(string connectionId, string id);
        Task Logout(string connectionId);
        Task Chat(string connectionId, string message);
    }

    public class ChatService : IChatService
    {
        private readonly IHubContext<ChatHub> _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly List<User> _users = new List<User>();

        public ChatService(
            IHubContext<ChatHub> context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Login(string connectionId, string id)
        {
            var code = 1;
            //var duplicationuser = GetUser(id);
            //if (duplicationuser != null)
            //{   //중복
            //    _users.Remove(duplicationuser);
            //}

            var user = new User()
            {
                ConnectionId = connectionId,
                Id = id,
            };

            _users.Add(user);

            var sc = new SC_Login()
            {
                code = code,
                User = user,
            };

            await _context.Clients.All.SendAsync("Login", sc);
            //await _context.Clients.Client(connectionId).SendAsync("Login", sc);
        }

        public async Task Logout(string connectionId)
        {
            var user = GetUserbyConnectionId(connectionId);
            if (user != null)
            {
                _users.Remove(user);
            }

            await Task.CompletedTask;
        }

        public async Task Chat(string connectionId, string message)
        {
            var sc = new SC_Chat()
            {
                Id = GetUserbyConnectionId(connectionId)?.Id,
                Message = message,
            };

            await _context.Clients.All.SendAsync("Chat", sc);

            LogService.Log(LogType.Chat, _httpContextAccessor.ToIP(), connectionId, sc);
        }

        private User GetUser(string id)
        {
            return _users.Find(x => x.Id == id);
        }

        private User GetUserbyConnectionId(string connectionId)
        {
            return _users.Find(x => x.ConnectionId == connectionId);
        }
    }
}
