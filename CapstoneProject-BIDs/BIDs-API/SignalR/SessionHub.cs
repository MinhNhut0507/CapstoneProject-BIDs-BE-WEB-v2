using Data_Access.Entities;
using Microsoft.AspNetCore.SignalR;

namespace BIDs_API.SignalR
{
    public class SessionHub:Hub
    {
        public async Task SessionAdd(Session Session)
        {
            await Clients.All.SendAsync("ReceiveSessionAdd", Session);
        }

        public async Task SessionUpdate(Session Session)
        {
            await Clients.All.SendAsync("ReceiveSessionUpdate", Session);
        }

        public async Task SessionDelete(Session Session)
        {
            await Clients.All.SendAsync("ReceiveSessionDelete", Session);
        }
    }
}
