using Data_Access.Entities;
using Microsoft.AspNetCore.SignalR;

namespace BIDs_API.SignalR
{
    public class BookingItemHub:Hub
    {
        public async Task BookingItemAdd(BookingItem BookingItem)
        {
            await Clients.All.SendAsync("ReceiveBookingItemAdd", BookingItem);
        }

        public async Task BookingItemUpdate(BookingItem BookingItem)
        {
            await Clients.All.SendAsync("ReceiveBookingItemUpdate", BookingItem);
        }

    }
}
