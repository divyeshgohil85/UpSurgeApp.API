using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UpSurgeApp.API.Dtos
{
    public class LoginDto
    {
        public string EmailOrMobile { get; set; }
        public string Password { get; set; }
        public bool IsEmail { get; set; }
        public bool IsMobile { get; set; }
        public Guid Udid { get; set; }
    }
}
