using Data_Access.Entities;
using Microsoft.AspNetCore.SignalR;

namespace BIDs_API.SignalR
{
    public class ItemDescriptionHub:Hub
    {
        public async Task ItemDescriptionAdd(ItemDescription ItemDescription)
        {
            await Clients.All.SendAsync("ReceiveItemDescriptionAdd", ItemDescription);
        }

        public async Task ItemDescriptionUpdate(ItemDescription ItemDescription)
        {
            await Clients.All.SendAsync("ReceiveItemDescriptionUpdate", ItemDescription);
        }
    }
}
