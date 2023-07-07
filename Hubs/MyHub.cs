using Microsoft.AspNetCore.SignalR;

namespace keyclock_Authentication.Hubs
{
    public class MyHub:Hub
    {
        public async Task SendMessage()
        {
            await Clients.All.SendAsync("ReceiveMessage", "Hey from Server");

        }

    }
}
