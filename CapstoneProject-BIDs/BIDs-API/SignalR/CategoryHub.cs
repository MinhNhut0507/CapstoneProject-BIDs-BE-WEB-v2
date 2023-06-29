using Data_Access.Entities;
using Microsoft.AspNetCore.SignalR;

namespace BIDs_API.SignalR
{
    public class CategoryHub:Hub
    {
        public async Task CategoryAdd(Category Category)
        {
            await Clients.All.SendAsync("ReceiveCategoryAdd", Category);
        }

        public async Task CategoryUpdate(Category Category)
        {
            await Clients.All.SendAsync("ReceiveCategoryUpdate", Category);
        }

        public async Task CategoryDelete(Category Category)
        {
            await Clients.All.SendAsync("ReceiveCategoryDelete", Category);
        }
    }
}
