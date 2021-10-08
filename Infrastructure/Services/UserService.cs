using Core.Common;
using Core.Entities;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Infrastructure.Services
{
    public interface IUserService
    {
        dynamic GetUsers(string emailAddress);
        Task AddUpdateUser(AppUser appUser);
        Task<StringMessageCL> BlockUserById(string Email, BlockUsers blockUser);


    }

    public class UserService : IUserService
    {
        private readonly UpSurgeAppDbContext _context;

        public UserService(UpSurgeAppDbContext context)
        {
            _context = context;
        }

        public dynamic GetUsers(string emailAddress)
        {
            var userId = _context.AppUsers.FirstOrDefault(x => x.Email == emailAddress).Id;

            var blockedUser = _context.BlockUsers.Where(x => x.UserId == userId).Select(x => x.BlockUserId).ToList();

            //var getUserId = _context.AppUsers.Select(x => x.Id).Except(blockedUser).ToList();
            var existingValues = _context.AppUsers
                .Select(x => new { x.Id, x.FirstName, x.LastName, x.Email, x.ProfilePictureUrl, x.Udid })
                .ToList();

            var filteredValues = existingValues.Where(x => !blockedUser.Contains(x.Id));

            //List<AppUserResponse> appUserResponse = new AppUserResponse();
            //appUserResponse = (AppUserResponse) existingValues;
            return filteredValues;
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

        public async Task<StringMessageCL> BlockUserById(string Email, BlockUsers blockUser)
        {
            try
            {
                var userId = _context.AppUsers.FirstOrDefault(x => x.Email == Email).Id;
                blockUser.UserId = userId;

                var checkBlockUser = _context.BlockUsers.FirstOrDefault(x => x.UserId == userId && x.BlockUserId == blockUser.BlockUserId);
                if (checkBlockUser == null)
                {
                    _context.BlockUsers.Add(blockUser);


                    await _context.SaveChangesAsync();
                    return new StringMessageCL("User Blocked Successfully", ResponseType.Success);
                }
                else
                {
                    return new StringMessageCL("User already Blocked",ResponseType.Failed);
                }


            }
            catch (Exception e)
            {
                return new StringMessageCL(e.Message, ResponseType.Failed);
            }

        }
    }

}
