using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UpSurgeApp.API.Dtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Please enter email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter first name")]
        [StringLength(200)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter last name")]
        [StringLength(200)]
        public string LastName { get; set; }

        [Required]
        [RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$"
        , ErrorMessage = "Password must have 1 Uppercase, 1 Lowercase, 1 number, 1 special character and atleast have 6 characters")]
        public string Password { get; set; }


        public string OTP { get; set; }

        [RegularExpression(@"^\+?\d{0,2}\-?\d{4,5}\-?\d{5,6}", ErrorMessage = "Please enter valid Mobile Number.")]
        public Int64 Mobile { get; set; }

        public bool IsAgreeToNotify { get; set; }

        public string Promocode { get; set; }

        public string ProfilePicture { get; set; }
        public bool IsPicSent { get; set; }

        public bool IsAgreeToTerms { get; set; }

        public int CountryId { get; set; }
    }
}
