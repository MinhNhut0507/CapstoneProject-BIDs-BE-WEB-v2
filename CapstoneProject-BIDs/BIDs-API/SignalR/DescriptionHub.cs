using Data_Access.Entities;
using Microsoft.AspNetCore.SignalR;

namespace BIDs_API.SignalR
{
    public class DescriptionHub:Hub
    {
        public async Task DescriptionAdd(Description Description)
        {
            await Clients.All.SendAsync("ReceiveDescriptionAdd", Description);
        }

        public async Task DescriptionUpdate(Description Description)
        {
            await Clients.All.SendAsync("ReceiveDescriptionUpdate", Description);
        }

        public async Task DescriptionDelete(Description Description)
        {
            await Clients.All.SendAsync("ReceiveDescriptionDelete", Description);
        }
    }
}
