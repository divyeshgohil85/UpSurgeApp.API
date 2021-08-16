using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interface;
using Infrastructure.Data;
using Infrastructure.Data.Identity;
using Infrastructure.Data.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace UpSurgeApp.API
{
    public class Program
    {
        private static readonly ILogRepository _log;
        public static async Task Main(string[] args)
        {

            var host = CreateHostBuilder(args).Build();

            /*Applying the migrations and creating the database at app startup */
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                var context = services.GetRequiredService<AppDbContext>();
                try
                {
                    /********** ADDS THE PENDING MIGRATIONS TO THE DATABASE AND IF DB DOESN'T EXISTS IT CREATES THE DB ********************/

                    await context.Database.MigrateAsync();

                    /* Seed The APP Data To the DATABASE  */

                    await AppDbContextSeed.SeedAsync(context, loggerFactory);


                    /********** Adds the Migrations for the IdentityContext *****************************/
                    //var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    //var identityContext = services.GetRequiredService<AppIdentityDbContext>();
                    //await identityContext.Database.MigrateAsync();
                    //await AppIdentityDbContextSeed.SeedUsersAsync(userManager);

                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error has occured during migration.");

                    LogService.Instance(context).AddErrorLogException(ex, "Program.cs");
                }

            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
