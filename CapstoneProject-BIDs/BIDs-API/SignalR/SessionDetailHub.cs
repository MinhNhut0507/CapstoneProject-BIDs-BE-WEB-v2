using Data_Access.Entities;
using Microsoft.AspNetCore.SignalR;

namespace BIDs_API.SignalR
{
    public class SessionDetailHub:Hub
    {
        public async Task SessionDetailAdd(SessionDetail SessionDetail)
        {
            await Clients.All.SendAsync("ReceiveSessionDetailAdd", SessionDetail);
        }

        public async Task SessionDetailUpdate(SessionDetail SessionDetail)
        {
            await Clients.All.SendAsync("ReceiveSessionDetailUpdate", SessionDetail);
        }

        public async Task SessionDetailDelete(SessionDetail SessionDetail)
        {
            await Clients.All.SendAsync("ReceiveSessionDetailDelete", SessionDetail);
        }
    }
}
