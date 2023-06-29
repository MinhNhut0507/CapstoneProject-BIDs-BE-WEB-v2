using Data_Access.Entities;
using Microsoft.AspNetCore.SignalR;

namespace BIDs_API.SignalR
{
    public class UserHub:Hub
    {
        public async Task UserAdd(User user)
        {
            await Clients.All.SendAsync("ReceiveUserAdd", user);
        }

        public async Task UserUpdate(User user)
        {
            await Clients.All.SendAsync("ReceiveUserUpdate", user);
        }

        public async Task UserActive(User user)
        {
            await Clients.All.SendAsync("ReceiveUserActive", user);
        }

        public async Task UserBan(User user)
        {
            await Clients.All.SendAsync("ReceiveUserBan", user);
        }

        public async Task UserUnban(User user)
        {
            await Clients.All.SendAsync("ReceiveUserUnban", user);
        }

        public async Task UserDeny(User user)
        {
            await Clients.All.SendAsync("ReceiveUserDeny", user);
        }
    }
}
