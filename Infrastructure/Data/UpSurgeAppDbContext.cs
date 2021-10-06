using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System.Linq;

namespace Infrastructure.Data
{
    public class UpSurgeAppDbContext : DbContext
    {
        public UpSurgeAppDbContext(DbContextOptions<UpSurgeAppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {

                property.SetColumnType("decimal(18,2)");
            }

            modelBuilder.Entity<EquityInfo>()
                .HasOne(x => x.Forecast)                
                .WithOne(x => x.EquityInfo)
                .HasPrincipalKey<EquityInfo>(x => x.StockTicker)
                .HasForeignKey<Forecast>(x => x.StockTicker);

            modelBuilder.Entity<MatchmakerUserPreference>()
                .Property(x => x.Values)
                .HasConversion(new ValueConverter<MatchmakerUserPreferencesValues, string>(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<MatchmakerUserPreferencesValues>(v)
                    ));

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<MobileOTP> MobileOTPs { get; set; }

        public DbSet<ContactUs> Contacts { get; set; }

        public DbSet<EquityInfo> EquityInfos { get; set; }
        public DbSet<SentioneTopic> SentioneTopics { get; set; }

        public DbSet<ForecastStep> ForecastSteps { get; set; }
        public DbSet<AnalystRating> AnalystRatings { get; set; }
        public DbSet<Forecast> Forecasts { get; set; }
        public DbSet<MatchmakerUserPreference> MatchmakerUserPreferences { get; set; }
        public DbSet<Channel> Channel { get; set; }


        //public DbSet<Core.Entities.Sygnal> Sygnals { get; set; }

        //public DbSet<SiteUser> SiteUsers { get; set; }

        //public DbSet<Role> Roles { get; set; }

        //public DbSet<Log> Logs { get; set; }

    }
}
