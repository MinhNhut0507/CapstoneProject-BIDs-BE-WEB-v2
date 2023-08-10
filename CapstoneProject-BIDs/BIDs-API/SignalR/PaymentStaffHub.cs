using Data_Access.Entities;
using Microsoft.AspNetCore.SignalR;

namespace BIDs_API.SignalR
{
    public class PaymentStaffHub:Hub
    {
        public async Task PaymentStaffAdd(PaymentStaff PaymentStaff)
        {
            await Clients.All.SendAsync("ReceivePaymentStaffAdd", PaymentStaff);
        }

        public async Task PaymentStaffUpdate(PaymentStaff PaymentStaff)
        {
            await Clients.All.SendAsync("ReceivePaymentStaffUpdate", PaymentStaff);
        }

        public async Task PaymentStaffDelete(PaymentStaff PaymentStaff)
        {
            await Clients.All.SendAsync("ReceivePaymentStaffDelete", PaymentStaff);
        }
    }
}
