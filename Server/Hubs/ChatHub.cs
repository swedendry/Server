using Microsoft.AspNetCore.SignalR;
using Server.Hubs.Models;
using Server.Services;
using System;
using System.Threading.Tasks;

namespace Server.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _service;

        public ChatHub(IChatService service)
        {
            _service = service;
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _service.Logout(Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }

        public void Login(CS_Login cs)
        {
            _service.Login(Context.ConnectionId, cs.Id);
        }

        public void Chat(CS_Chat cs)
        {
            _service.Chat(Context.ConnectionId, cs.Message);
        }
    }
}
