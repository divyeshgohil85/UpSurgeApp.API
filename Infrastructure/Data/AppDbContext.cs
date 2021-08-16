using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<MobileOTP> MobileOTPs { get; set; }

        public DbSet<ContactUs> Contacts { get; set; }



        //public DbSet<SiteUser> SiteUsers { get; set; }

        //public DbSet<Role> Roles { get; set; }

        //public DbSet<Log> Logs { get; set; }

    }
}
