using Data_Access.Entities;
using Microsoft.AspNetCore.SignalR;

namespace BIDs_API.SignalR
{
    public class ItemHub:Hub
    {
        public async Task ItemAdd(Item Item)
        {
            await Clients.All.SendAsync("ReceiveItemAdd", Item);
        }

        public async Task ItemUpdate(Item Item)
        {
            await Clients.All.SendAsync("ReceiveItemUpdate", Item);
        }

        public async Task ItemDelete(Item Item)
        {
            await Clients.All.SendAsync("ReceiveItemDelete", Item);
        }
    }
}
