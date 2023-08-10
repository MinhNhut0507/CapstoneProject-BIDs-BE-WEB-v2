using Data_Access.Entities;
using Microsoft.AspNetCore.SignalR;

namespace BIDs_API.SignalR
{
    public class UserPaymentInformationHub:Hub
    {
        public async Task UserPaymentInformationAdd(UserPaymentInformation UserPaymentInformation)
        {
            await Clients.All.SendAsync("ReceiveUserPaymentInformationAdd", UserPaymentInformation);
        }

        public async Task UserPaymentInformationUpdate(UserPaymentInformation UserPaymentInformation)
        {
            await Clients.All.SendAsync("ReceiveUserPaymentInformationUpdate", UserPaymentInformation);
        }

        public async Task UserPaymentInformationDelete(UserPaymentInformation UserPaymentInformation)
        {
            await Clients.All.SendAsync("ReceiveUserPaymentInformationDelete", UserPaymentInformation);
        }
    }
}
