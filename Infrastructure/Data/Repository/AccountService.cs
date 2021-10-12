using Core.Common;
using Core.Entities;
using Core.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;

namespace Infrastructure.Data.Repository
{
    public class AccountService : IAccountRepository
    {

        private readonly IConfiguration _configuration;

        private readonly UpSurgeAppDbContext _context;

        public AccountService(UpSurgeAppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<StringMessageCL> CheckEmailExistsAsync(string Email)
        {
            try
            {
                var user = await _context.AppUsers.Where(p => p.Email == Email).FirstOrDefaultAsync();
                if (user == null)
                {
                    return new StringMessageCL("", ResponseType.Success);
                }
                else if (user.IsActive && user.Id > 0)
                {
                    return new StringMessageCL("Email Address has already been taken, please try with other email address !!!.", ResponseType.Failed);
                }

                return new StringMessageCL("Failed", ResponseType.Failed);
            }
            catch (Exception ex)
            {
                LogService.Instance(_context).AddErrorLogException(ex, "AccountService.cs");
                return new StringMessageCL(ex.Message, ResponseType.Exception);
            }
        }

        public async Task<StringMessageCL> AddNewUser(AppUser user, string OTP)
        {
            var OTPInt = Convert.ToInt32(OTP);

            try
            { //Check user's OTP and make active

                var mobileOTPData = await _context.MobileOTPs.Where
                    (p => p.Mobile == user.Mobile && p.OTP == OTPInt && p.IsVerified == false).OrderBy(p => p.Id).LastOrDefaultAsync();

                if (mobileOTPData == null)
                {
                    return new StringMessageCL("The OTP didn't match with the OTP sent on your mobile, Retry", ResponseType.Failed);
                }
                else if (mobileOTPData.Id > 0)
                {
                    mobileOTPData.IsVerified = true;
                    await _context.SaveChangesAsync();
                }

                //user.CreatedAt = DateTime.UtcNow;
                user.UserName = user.FirstName + " " + user.LastName;
                user.LastLoginTime = DateTime.UtcNow;
                user.Password = HashPassword(user.Password);
                user.LastLoginTime = DateTime.UtcNow;
                user.Token = user.Token;//Guid.NewGuid().ToString();
                await _context.AppUsers.AddAsync(user);
                await _context.SaveChangesAsync();

                return new StringMessageCL("User Created Successfully.", ResponseType.Success, user.Token);
            }
            catch (Exception ex)
            {
                LogService.Instance(_context).AddErrorLogException(ex, "AccountService.cs");
                return new StringMessageCL(ex.Message, ResponseType.Exception); ;
            }
        }

        public static string HashPassword(string password)
        {
            byte[] arrbyte = new byte[password.Length];
            using (SHA1 hash = new SHA1CryptoServiceProvider())
            {
                arrbyte = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
            return Convert.ToBase64String(arrbyte);
        }

        public async Task<StringMessageCL> SendOTPToMobile(MobileOTP mobileOTPdetails, string MobileWithCountryCode)
        {

            if (mobileOTPdetails.Mobile == 0)
            {
                return new StringMessageCL("Mobile number is empty, Please resend the details.", ResponseType.Failed);
            }

            try
            {
                string OTP = Convert.ToString(GenerateRandomNo());
                //Send SMS on Mobile Number
                string ifSent = TwilioSMSAPI(MobileWithCountryCode, OTP);

                if (string.IsNullOrEmpty(ifSent))
                {
                    LogService.Instance(_context).AddErrorLog("$$Twilio Error --AccountService/MobileOTP", "MobileOtp");
                    return new StringMessageCL("Twilio couldn't send the message, Please contatct administrator.", ResponseType.Failed);
                }

                mobileOTPdetails.CreatedAt = DateTime.UtcNow;
                mobileOTPdetails.IsActive = true;
                mobileOTPdetails.IsDeleted = false;
                mobileOTPdetails.IsVerified = false;
                mobileOTPdetails.OTP = Convert.ToInt32(OTP);

                _context.MobileOTPs.Add(mobileOTPdetails); // Save OTP sent to Mobile 
                await _context.SaveChangesAsync();
                return new StringMessageCL("Mobile OTP has been sent", ResponseType.Success, "", mobileOTPdetails.OTP);
            }
            catch (ApiException e)
            {
                LogService.Instance(_context).AddErrorLogException(e, "$$Twilio Error --AccountService/MobileOTP");
                return new StringMessageCL(e.Message, ResponseType.Exception);
            }
            catch (Exception ex)
            {
                LogService.Instance(_context).AddErrorLogException(ex, "MobileOTP");
                return new StringMessageCL(ex.Message, ResponseType.Exception);
            }
        }

        //Generate RandomNo
        private int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        private string TwilioSMSAPI(string MobileWithCountryCode, string OTP)
        {
            string accountSid = _configuration["Twilio:TWILIO_ACCOUNT_SID"];                    // Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
            string authToken = _configuration["Twilio:TWILIO_AUTH_TOKEN"];
            string fromMobileNumber = _configuration["Twilio:FROM_MOBILE"];

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: "Your UpSurge Verification code is:  " + OTP + ".",
                from: new Twilio.Types.PhoneNumber(fromMobileNumber),
                to: new Twilio.Types.PhoneNumber(MobileWithCountryCode)
            );

            string ifSent = message.Sid;
            return ifSent;
        }

        public async Task<StringMessageCL> FindByEmailOrMobileAsync(string EmailOrMobile, bool IsMobile, bool IsEmail)
        {
            try
            {
                string Email = string.Empty;
                string Mobile = string.Empty;
                AppUser user = new AppUser();

                if (IsEmail)
                {
                    user = await _context.AppUsers.Where(p => p.Email == EmailOrMobile).FirstOrDefaultAsync();
                }
                if (IsMobile)
                {
                    var mobile = Convert.ToInt32(EmailOrMobile);
                    user = await _context.AppUsers.Where(p => p.Mobile == mobile).FirstOrDefaultAsync();
                }
                if (user == null)
                {
                    return new StringMessageCL("There is no user with Email and/or Mobile in the system. ", ResponseType.Failed);
                }
                else if (user.IsActive && user.Id > 0)
                {
                    return new StringMessageCL("", ResponseType.Success);
                }

                return new StringMessageCL("Failed", ResponseType.Failed);
            }
            catch (Exception ex)
            {
                LogService.Instance(_context).AddErrorLogException(ex, "AccountService.cs/FindByEmailOrMobileAsync");
                return new StringMessageCL(ex.Message, ResponseType.Exception);
            }
        }

        public async Task<UserDetails> CheckPasswordSignInAsync(string Password)
        {
            //UserDetails UserDetails  = new UserDetails();
            try
            {
                var HashedPassword = HashPassword(Password);
                var user = await _context.AppUsers.Where(p => p.Password == HashedPassword).FirstOrDefaultAsync();
                if (user == null)
                {
                    return new UserDetails(user, "Password is invalid", ResponseType.Failed);
                }
                if (user != null)
                {
                    return new UserDetails(user, "Success", ResponseType.Success);
                }
                return new UserDetails(user, "Success", ResponseType.Success);
            }
            catch (Exception ex)
            {
                LogService.Instance(_context).AddErrorLogException(ex, "AccountService.cs/FindByEmailOrMobileAsync");
                return new UserDetails(null, ex.Message, ResponseType.Exception);
            }
        }

        public async Task<StringMessageCL> SendVerificationCode(string Email)
        {
            try
            {
                if (!string.IsNullOrEmpty(Email))
                {
                    var result = await _context.AppUsers.Where(p => p.Email == Email).FirstOrDefaultAsync();
                    if (result == null)
                    {
                        return new StringMessageCL("No account exists with this Email !!!.", ResponseType.Failed);
                    }

                    var verificationCode = GenerateRandomNo();

                    var IsSent = await SendEmailTo(Email, verificationCode);
                    if (IsSent)
                    {
                        var mobileOTPdetails = new MobileOTP
                        {
                            CountryId = result.CountryId,
                            Mobile = result.Mobile,
                            OTP = verificationCode
                        };

                        _context.MobileOTPs.Add(mobileOTPdetails);

                        await _context.SaveChangesAsync();

                        return new StringMessageCL("Verification Code sent successfully !!!.", ResponseType.Success,"", verificationCode);
                    }
                    else {
                        return new StringMessageCL("An error has occurred !!!.", ResponseType.Failed);
                    }
                }
                return new StringMessageCL("Email is not correct !!!.", ResponseType.Failed);
            }
            catch (Exception ex)
            {

                LogService.Instance(_context).AddErrorLogException(ex, "AccountService.cs/ResetPassword");
                return new StringMessageCL(ex.Message, ResponseType.Exception);
            }
        }

        public bool VerifyCode(string email, int code)
        {
            var appUser = _context.AppUsers.FirstOrDefault(p => p.Email == email);
            if (appUser == null)
            {
                return false;
            }

            var otp = _context.MobileOTPs.FirstOrDefault(
                x => x.IsActive && !x.IsDeleted && !x.IsVerified && x.Mobile == appUser.Mobile && x.OTP == code);

            if (otp == null)
                return false;

            otp.IsVerified = true;
            _context.SaveChanges();

            return true;
        }


        public async Task<StringMessageCL> ResetPassword(string Email, string Password)
        {
            try
            {
                var user = await _context.AppUsers.Where(p => p.Email == Email).FirstOrDefaultAsync();
                if (user == null)
                {
                    return new StringMessageCL("There is no account linked with the Email exists in our system !!!.", ResponseType.Failed);
                }
                if (user.IsActive)
                {
                    user.Password = HashPassword(Password);
                    user.PasswordChangeDate = DateTime.UtcNow;
                    user.UpdatedAt = DateTime.UtcNow;
                    _context.SaveChanges();
                }

                return new StringMessageCL("Password changed successfully !!!.", ResponseType.Success);
            }

            catch (Exception ex)
            {
                LogService.Instance(_context).AddErrorLogException(ex, "AccountService.cs/ResetPassword");
                return new StringMessageCL(ex.Message, ResponseType.Exception);
            }
        }

        private async Task<bool> SendEmailTo(string Email, int Code)
        
        {
            try
            {                
                List<string> EmailInfo = new List<string>();
                EmailInfo.Add(Email.Trim());

                Mail mail = new Mail(_configuration);
                string content = "";
                content = FormatText.Instance.EmailTemplate("UpSurge Administrator", "Password Recovery", Code.ToString(), "");

                StringMessageCL returnMessage = await mail.SendEmail("Recovery Email", content, true, EmailInfo, "");

               // var res = mail.SendEmailTest(content, "");

                if (returnMessage.Response == ResponseType.Success)
                {
                    return true;
                }
                else
                {
                    LogService.Instance(_context).AddErrorLog("Error Occurred, Couldn't send email to "+Email, "AccountService.cs/Email Send for verificationcode");
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogService.Instance(_context).AddErrorLogException(ex, "AccountService.cs/Email Send for verificationcode");
                return false;
            }

        }

        public async Task<StringMessageCL> UpdateMembership(string Email, int membershipId)
        {
            try
            {
                var user = await _context.AppUsers.Where(p => p.Email == Email).FirstOrDefaultAsync();
                if (user == null)
                {
                    return new StringMessageCL("There is no account linked with the Email exists in our system !!!.", ResponseType.NotFound);
                }

                user.MembershipId = membershipId;
                user.UpdatedAt = DateTime.UtcNow;

                _context.SaveChanges();

                return new StringMessageCL("Password changed successfully !!!.", ResponseType.Success);
            }

            catch (Exception ex)
            {
                LogService.Instance(_context).AddErrorLogException(ex, "AccountService.cs/ResetPassword");
                return new StringMessageCL(ex.Message, ResponseType.Exception);
            }
        }

        public IEnumerable<(string Email, string DisplayName, int? MembershipId, string ProfilePictureUrl,Guid Udid)> GetUsersByMembershipId(int membershipId = -1)
        {
            try
            {
                var userId = _context.AppUsers.FirstOrDefault(x => x.MembershipId == membershipId).Id;

                var blockedUser = _context.BlockUsers.Where(x => x.UserId == userId).Select(x => x.BlockUserId).ToList();

                var existingValues = _context.AppUsers.Where(
                        x => 
                            membershipId == -1 ||
                            (x.MembershipId == null && membershipId == 0) || 
                            (x.MembershipId != null && x.MembershipId == membershipId)
                    )
                    .ToArray();                    

                var filteredValues = existingValues.Where(x => !blockedUser.Contains(x.Id)).Select(x => (x.Email, x.FirstName, x.MembershipId,x.ProfilePictureUrl,x.Udid));

                return filteredValues;
            }
            catch (Exception ex)
            {
                LogService.Instance(_context).AddErrorLogException(ex, "AccountService.cs/ResetPassword");
                return default;
            }
        }

        public async Task<StringMessageCL> CheckAndUpdateUdIDAsync(string Email,string Udid)
        {
            try
            {
                var user = await _context.AppUsers.Where(p => p.Email == Email).FirstOrDefaultAsync();
                if (user != null)
                {
                    if(user.Udid.ToString() != Udid)
                    {
                        user.Udid = Guid.Parse(Udid); 
                        _context.SaveChanges();
                    }
                    return new StringMessageCL("", ResponseType.Success);
                }
                
                return new StringMessageCL("Failed", ResponseType.Failed);
            }
            catch (Exception ex)
            {
                LogService.Instance(_context).AddErrorLogException(ex, "AccountService.cs");
                return new StringMessageCL(ex.Message, ResponseType.Exception);
            }
        }

    }
}


