using Data_Access.Entities;
using Microsoft.AspNetCore.SignalR;

namespace BIDs_API.SignalR
{
    public class ImageHub:Hub
    {
        public async Task ImageAdd(Image Image)
        {
            await Clients.All.SendAsync("ReceiveImageAdd", Image);
        }

        public async Task ImageUpdate(Image Image)
        {
            await Clients.All.SendAsync("ReceiveImageUpdate", Image);
        }

        public async Task ImageDelete(Image Image)
        {
            await Clients.All.SendAsync("ReceiveImageDelete", Image);
        }
    }
}
