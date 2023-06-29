using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using System;
using Polly;

namespace Data_Access.Extension
{
    public static class HostExtension
    {
        public static IHost MigrateDbContext<TConText>(this IHost host, Action<TConText, IServiceProvider> seeder) where TConText : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TConText>>();
                var context = services.GetService<TConText>();

                try
                {
                    logger.LogInformation("Migrating database associated with context {BIDs}", typeof(TConText).Name);

                    var retries = 10;
                    var retry = Policy.Handle<SqlException>()
                        .WaitAndRetry(
                        retryCount: retries,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (exception, timeSpan, retry, ctx) =>
                        {
                            logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry}", typeof(TConText).Name);
                        });
                    retry.Execute(() => InvokeSeeder(seeder, context, services));

                    logger.LogInformation("Migrating database associated with context {BIDs}", typeof(TConText).Name);
                }
                catch(Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating on database used on context {BIDs}", typeof(TConText).Name);
                }
            }
            return host;
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider service)
            where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, service);
        }
    }
}
