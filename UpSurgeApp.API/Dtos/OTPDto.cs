using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UpSurgeApp.API.Dtos
{
    public class OTPDto
    {
        public int CountryId { get; set; }
        [Required(ErrorMessage = "Please enter Mobile Number")]
        [RegularExpression(@"^\+?\d{0,2}\-?\d{4,5}\-?\d{5,6}", ErrorMessage ="Please enter valid Mobile Number.")]
        public  Int64 Mobile { get; set; }
        public string CountryCode { get; set; }
    }

    public class OTPReturnDto {

        public int OTP { get; set; }
        public bool IsSent { get; set; }

    }
}
