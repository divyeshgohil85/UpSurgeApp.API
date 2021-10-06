using Core.Entities;
using Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Infrastructure.Services
{
    public interface IUserService
    {
        dynamic GetUsers();
        Task AddUpdateUser(AppUser appUser);
    }

    public class UserService : IUserService
    {
        private readonly UpSurgeAppDbContext _context;

        public UserService(UpSurgeAppDbContext context)
        {
            _context = context;
        }

        public dynamic GetUsers()
        {
            var existingValues =  _context.AppUsers.Where(x=>x.IsBlocked == false)
                .Select(x => new { x.Id, x.FirstName,x.LastName,x.Email,x.ProfilePictureUrl,x.Udid })
                .ToList();

            //List<AppUserResponse> appUserResponse = new AppUserResponse();
            //appUserResponse = (AppUserResponse) existingValues;
            return existingValues;
        }

        public async Task AddUpdateUser(AppUser appUser)
        {
            var existingValues = _context.AppUsers.FirstOrDefault(x => x.Id == appUser.Id);
            if (existingValues == null)
            {
                _context.AppUsers.Add(appUser);
            }
            else
            {
                existingValues = appUser;
            }

            await _context.SaveChangesAsync();
        }
    }

}
