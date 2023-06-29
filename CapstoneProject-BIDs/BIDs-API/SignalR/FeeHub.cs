using Data_Access.Entities;
using Microsoft.AspNetCore.SignalR;

namespace BIDs_API.SignalR
{
    public class FeeHub:Hub
    {
        public async Task FeeAdd(Fee Fee)
        {
            await Clients.All.SendAsync("ReceiveFeeAdd", Fee);
        }

        public async Task FeeUpdate(Fee Fee)
        {
            await Clients.All.SendAsync("ReceiveFeeUpdate", Fee);
        }

        public async Task FeeDelete(Fee Fee)
        {
            await Clients.All.SendAsync("ReceiveFeeDelete", Fee);
        }
    }
}
