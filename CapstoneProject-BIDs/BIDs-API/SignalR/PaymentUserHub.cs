using Data_Access.Entities;
using Microsoft.AspNetCore.SignalR;

namespace BIDs_API.SignalR
{
    public class PaymentUserHub:Hub
    {
        public async Task PaymentUserAdd(PaymentUser PaymentUser)
        {
            await Clients.All.SendAsync("ReceivePaymentUserAdd", PaymentUser);
        }

        public async Task PaymentUserUpdate(PaymentUser PaymentUser)
        {
            await Clients.All.SendAsync("ReceivePaymentUserUpdate", PaymentUser);
        }

        public async Task PaymentUserDelete(PaymentUser PaymentUser)
        {
            await Clients.All.SendAsync("ReceivePaymentUserDelete", PaymentUser);
        }
    }
}
