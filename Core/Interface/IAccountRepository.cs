using Core.Common;
using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface IAccountRepository
    {
        public Task<StringMessageCL> CheckEmailExistsAsync(string Email);

        public Task<StringMessageCL> AddNewUser(AppUser user, string OTP);

        public Task<StringMessageCL> SendOTPToMobile(MobileOTP mobileOTP, string MobileWithCountryCode);

        public Task<StringMessageCL> FindByEmailOrMobileAsync(string EmailOrMobile, bool IsMobile, bool IsEmail);

        public Task<UserDetails> CheckPasswordSignInAsync(string Password);

        public Task<StringMessageCL> SendVerificationCode(string Email);

        bool VerifyCode(string email, int code);

        public Task<StringMessageCL> UpdateMembership(string Email, int membershipId);

        public IEnumerable<(string Email, string DisplayName, int? MembershipId)> GetUsersByMembershipId(int membershipId = -1);

        public Task<StringMessageCL> CheckAndUpdateUdIDAsync(string Email,string Udid);

    }
}
