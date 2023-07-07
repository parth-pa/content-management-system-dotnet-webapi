using keyclock_Authentication.Hubs;
using Microsoft.AspNetCore.SignalR;
using Npgsql;

namespace keyclock_Authentication
{
    public class NotificationService:BackgroundService
    {

        public readonly IConfiguration _Configuration;
        private readonly IHubContext<MyHub> _hubContext;

        public NotificationService(IHubContext<MyHub> hubContext, IConfiguration configuration) {

            _hubContext = hubContext;
            _Configuration = configuration;

        }

        public async Task StartListingForNotifications()
        {
            string con = _Configuration.GetConnectionString("conn");
            NpgsqlConnection conn = new NpgsqlConnection(con);

            await conn.OpenAsync();

            using(var cmd = new NpgsqlCommand("Listen telegram", conn))
            {
                cmd.ExecuteNonQuery();
            }

            conn.Notification += (sender, e) =>
            {
                _hubContext.Clients.All.SendAsync("ReceiveMessage", e.Payload);
            };

            while (true)
            {
                conn.Wait();
            }



        }




        private readonly PeriodicTimer timer = new(TimeSpan.FromMilliseconds(100));


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await StartListingForNotifications();
            }
        }




        //private void Conn_Notification(object sender, NpgsqlNotificationEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
