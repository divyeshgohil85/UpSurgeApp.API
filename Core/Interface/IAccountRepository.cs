using Core.Common;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
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

        public Task<StringMessageCL> ResetPassword(string Email);


    }
}
