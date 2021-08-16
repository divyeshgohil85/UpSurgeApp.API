using Core.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Core.Interface;
using Infrastructure.Data.Repository;
using Microsoft.AspNetCore.Hosting;

namespace Infrastructure.Data
{
    public class AppDbContextSeed
    {

        public static async Task SeedAsync(AppDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                /*  Seeding Countries into the DB  */
                if (!context.Countries.Any())
                {
                    var countryData =
                        File.ReadAllText("./wwwroot/seeddata/country.json");
                    var country = JsonConvert.DeserializeObject<List<Country>>(countryData);
                    /*To test exception*/
                    //var country = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countryData);
                    foreach (var item in country)
                    {
                        context.Countries.Add(item);
                    }
                    // context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Students ON");
                    await context.SaveChangesAsync();
                }

                //Seed Membership data to DB

                if (!context.Memberships.Any())
                {
                    var memberData =
                        File.ReadAllText("./wwwroot/seeddata/membership.json");
                    var memberships = JsonConvert.DeserializeObject<List<Membership>>(memberData);
                    foreach (var item in memberships)
                    {
                        context.Memberships.Add(item);
                    }

                    await context.SaveChangesAsync();
                }

            }

            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<AppDbContextSeed>();
                logger.LogError(ex.Message);

                LogService.Instance(context).AddErrorLogException(ex, "AppDbContextSeed.cs");

            }
        }
    }
}
