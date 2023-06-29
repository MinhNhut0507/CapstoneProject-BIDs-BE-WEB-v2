using Data_Access.Entities;
using Microsoft.AspNetCore.SignalR;

namespace BIDs_API.SignalR
{
    public class BanHistoryHub:Hub
    {
        public async Task BanHistoryAdd(BanHistory BanHistory)
        {
            await Clients.All.SendAsync("ReceiveBanHistoryAdd", BanHistory);
        }

        public async Task BanHistoryUpdate(BanHistory BanHistory)
        {
            await Clients.All.SendAsync("ReceiveBanHistoryUpdate", BanHistory);
        }

    }
}
