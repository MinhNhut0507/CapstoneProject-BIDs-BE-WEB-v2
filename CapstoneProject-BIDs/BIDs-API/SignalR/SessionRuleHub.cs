using Data_Access.Entities;
using Microsoft.AspNetCore.SignalR;

namespace BIDs_API.SignalR
{
    public class SessionRuleHub:Hub
    {
        public async Task SessionRuleAdd(SessionRule SessionRule)
        {
            await Clients.All.SendAsync("ReceiveSessionRuleAdd", SessionRule);
        }

        public async Task SessionRuleUpdate(SessionRule SessionRule)
        {
            await Clients.All.SendAsync("ReceiveSessionRuleUpdate", SessionRule);
        }

        public async Task SessionRuleDelete(SessionRule SessionRule)
        {
            await Clients.All.SendAsync("ReceiveSessionRuleDelete", SessionRule);
        }
    }
}
