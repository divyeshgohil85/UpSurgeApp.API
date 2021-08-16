using Core.Interface;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using UpSurgeApp.API.Dtos;
using UpSurgeApp.API.Extensions;
using UpSurgeApp.API.Errors;
using Core.Entities;
using Infrastructure.Data.Repository;
using static UpSurgeApp.API.Errors.ApiResponse;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Microsoft.Extensions.Configuration;
using Core.Common;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Text.RegularExpressions;

namespace UpSurgeApp.API.Controllers
{

    public class AccountController : BaseApiController
    {
        //private readonly UserManager<AppUser> _userManager;
        //private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;


        private readonly IAccountRepository _accRepo;

        public AccountController(IAccountRepository accRepo,
                IMapper mapper, IWebHostEnvironment env, ITokenService tokenService)
        {
            //_userManager = userManager;
            //_signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _accRepo = accRepo;
            _env = env;

        }

        bool IsValidEmail(string strIn)
        {
            // Return true if strIn is in valid e-mail format.
            try
            {
                return Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        bool IsValidMobile(string strMobile)
        {

            try
            {
                //@" ^\+[1-9]{1}[0-9]{3,14}$
                //return Regex.IsMatch(strMobile, ");
                return Regex.IsMatch(strMobile, @"^\+?\d{0,2}\-?\d{4,5}\-?\d{5,6}$");
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[]
                 { "There are some errors, Model State is not valid."}
                });
            }

            if (loginDto.IsEmail)
            {
                var isValidEmail = IsValidEmail(loginDto.EmailOrMobile);
                if (!isValidEmail)
                {
                    return new BadRequestObjectResult(new ApiValidationErrorResponse
                    {
                        Errors = new[]
                     { "There are some errors, Email is not valid."}
                    });
                }
            }

            if (loginDto.IsMobile)
            {

                var isValidMobile = IsValidMobile(loginDto.EmailOrMobile);
                if (!isValidMobile)
                {
                    return new BadRequestObjectResult(new ApiValidationErrorResponse
                    {
                        Errors = new[]
                     { "There are some errors, Mobile Number is not valid."}

                    });
                }
            }
            var user = await _accRepo.FindByEmailOrMobileAsync(loginDto.EmailOrMobile, loginDto.IsMobile, loginDto.IsEmail);

            //CHECK USER EMAIL AND MOBILE
            if (user.Response == ResponseType.Failed || user.Response == ResponseType.Exception)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[]
                  { user.Message }
                });
            }
            //CHECK PASSWORD
            var result = await _accRepo.CheckPasswordSignInAsync(loginDto.Password);

            if (result.Response == ResponseType.Failed || result.Response == ResponseType.Exception)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[]
                 { result.Message }
                });
            }
            else if (result.User != null && result.Response == ResponseType.Success)
            {
                return new UserDto
                {
                    Email = result.User.Email,
                    DisplayName = result.User.FirstName,
                    Token = result.User.Token,
                    MembershipId = result.User.MembershipId

                };
            }

            return new BadRequestObjectResult(new ApiValidationErrorResponse
            {
                Errors = new[]
                     { "There are some errors, Model State is not valid."}
            });
        }


        private bool SaveImage(string base64img, string imageBasePath, string outputImgFilename = "image.png")
        {
            var folderPath = System.IO.Path.Combine(_env.ContentRootPath, "ProfilePics/" + imageBasePath);
            if (!System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            System.IO.File.WriteAllBytes(Path.Combine(folderPath, outputImgFilename), Convert.FromBase64String(base64img));

            return true;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new BadRequestObjectResult(new ApiValidationErrorResponse
                    {
                        Errors = new[]
                     { "There are some errors, Model State is not valid."}
                    });
                }

                // CheckEmailExistsOrNot
                StringMessageCL isEmailExists = await _accRepo.CheckEmailExistsAsync(registerDto.Email);

                //CheckMobileExistsOrNot


                if (isEmailExists.Response == ResponseType.Failed || isEmailExists.Response == ResponseType.Exception)
                {
                    return new BadRequestObjectResult(new ApiValidationErrorResponse
                    {
                        Errors = new[]
                       { isEmailExists.Message}
                    });
                }


                /* Need to check this issue */


                //if (registerDto.IsPicSent)
                //{
                //    //validate base64string
                //    byte[] bytes = null;
                //    var isValid = TryGetFromBase64String(registerDto.ProfilePicture, out bytes);
                //    if (!isValid)
                //    {
                //        return new BadRequestObjectResult(new ApiValidationErrorResponse
                //        {
                //            Errors = new[]
                //        { "The Profile Pic Base64 string is not valid."}
                //        });

                //    }
                //}

                //string fileName = "";
                //var fileNameOutput = SaveImage(registerDto.ProfilePicture, registerDto.FirstName, fileName);




                //Map New User
                var _mappedNewuser = _mapper.Map<RegisterDto, AppUser>(registerDto);

                //Add Data to DB
                _mappedNewuser.Token = _tokenService.CreateToken(_mappedNewuser);
                var result = await _accRepo.AddNewUser(_mappedNewuser, registerDto.OTP);

                if (result.Response == ResponseType.Exception || result.Response == ResponseType.Failed)
                {
                    return new BadRequestObjectResult(new ApiValidationErrorResponse
                    {
                        Errors = new[]
                     { result.Message}
                    });
                }

                else if (result.Response == ResponseType.Success)
                {
                    return Ok(new UserDto
                    {
                        DisplayName = _mappedNewuser.FirstName,
                        Token = result.RetValue, //Token
                        Email = _mappedNewuser.Email,
                        MembershipId = result.IntValue //Membership

                    });
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                //  LogService.Instance(context)
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[]
                     { ex.Message}
                });
            }
        }

        [Authorize]
        [HttpGet("SendResetPasswordLink")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StringMessageCL>> SendResetPasswordLink([FromQuery] string EmailAddress)
        {


            try
            {

                if (!ModelState.IsValid)
                {
                    return new BadRequestObjectResult(new ApiValidationErrorResponse
                    {
                        Errors = new[]
                          { "Please enter the valid email address !!!." }
                    });
                }

                if (string.IsNullOrEmpty(EmailAddress))
                {
                    return new BadRequestObjectResult(new ApiValidationErrorResponse
                    {
                        Errors = new[]
                          { "Please enter email address to reset !!!." }
                    });
                }

                var IsValidEmailAddress = IsValidEmail(EmailAddress);

                if (!IsValidEmailAddress)
                {
                    return new BadRequestObjectResult(new ApiValidationErrorResponse
                    {
                        Errors = new[]
                          { "Please enter the valid email address !!!." }
                    });
                }


                var result = await _accRepo.SendVerificationCode(EmailAddress);


                if (result.Response == ResponseType.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return new BadRequestObjectResult(new ApiValidationErrorResponse
                    {
                        Errors = new[]

                          { result.Message }
                    });
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[]
                      { ex.Message }
                });
            }
        }


        [Authorize]
        [HttpPost("ResetPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StringMessageCL>> ResetPassword([FromQuery] string EmailAddress, string Password, string NewPassword)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return new BadRequestObjectResult(new ApiValidationErrorResponse
                    {
                        Errors = new[]
                          { "Please enter the valid email address !!!." }
                    });
                }

                if (string.IsNullOrEmpty(EmailAddress))
                {
                    return new BadRequestObjectResult(new ApiValidationErrorResponse
                    {
                        Errors = new[]
                          { "Please enter email address to reset !!!." }
                    });
                }

                var IsValidEmailAddress = IsValidEmail(EmailAddress);

                if (!IsValidEmailAddress)
                {
                    return new BadRequestObjectResult(new ApiValidationErrorResponse
                    {
                        Errors = new[]
                          { "Please enter the valid email address !!!." }
                    });
                }


                var result = await _accRepo.SendVerificationCode(EmailAddress);


                if (result.Response == ResponseType.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return new BadRequestObjectResult(new ApiValidationErrorResponse
                    {
                        Errors = new[]

                          { result.Message }
                    });
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[]
                      { ex.Message }
                });
            }
        }





        private static bool TryGetFromBase64String(string base64, out byte[] output)
        {
            output = null;
            try
            {
                output = Convert.FromBase64String(base64);
                return true;
            }
            catch (FormatException ex)
            {
                return false;
            }
        }


        [HttpPost("sendotp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StringMessageCL>> SendOTP([FromBody] OTPDto OTPDetails)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[]
                      { "Please validate the phone number and country. !!!." }
                });
            }

            try
            {
                //Get CountryCode and Mobile number from device
                string MobileWithCountryCode = string.Concat(OTPDetails.CountryCode, OTPDetails.Mobile);

                var mappedData = _mapper.Map<OTPDto, MobileOTP>(OTPDetails);

                var result = await _accRepo.SendOTPToMobile(mappedData, MobileWithCountryCode);
                if (result.Response == ResponseType.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return new BadRequestObjectResult(new ApiValidationErrorResponse
                    {
                        Errors = new[]
                          { result.Message }
                    });
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[]
                      { ex.Message }
                });
            }
        }






        //[Authorize]
        //[HttpGet]
        //public async Task<ActionResult<UserDto>> GetCurrentUser()
        //{
        //    var user = await _userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);

        //    return new UserDto
        //    {
        //        Email = user.Email,
        //        DisplayName = user.UserName,
        //        Token = _tokenService.CreateToken(user)
        //    };
        //}

    }
}
