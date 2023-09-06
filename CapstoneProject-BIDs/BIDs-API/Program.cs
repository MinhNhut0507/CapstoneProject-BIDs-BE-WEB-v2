using Data_Access.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Data_Access.Extension;
using BIDs_API.SignalR;

namespace BIDs_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            ////host.MigrateDbContext<BIDsContext>((context, services) => 
            ////{
            ////    var logger = services.GetService<ILogger<BIDsContextSeed>>(); 
            ////    new BIDsContextSeed().SeedAsync(context, logger).Wait();
            ////});
            //var builder = WebApplication.CreateBuilder(args);
            //builder.Services.AddSignalR();
            //var app = builder.Build();
            //if(!app.Environment.IsDevelopment())
            //{
            //    app.UseExceptionHandler("/Error");
            //    app.UseHsts();
            //}
            //app.UseHttpsRedirection();
            //app.UseStaticFiles();
            //app.UseRouting();
            //app.MapHub<SessionDetailHub>("/sessiondetailhub");
            //app.Run();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConsole(); // Thêm nhà cung cấp log Console
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
