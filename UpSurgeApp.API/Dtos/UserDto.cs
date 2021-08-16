using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UpSurgeApp.API.Dtos
{
    public class UserDto
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Token { get; set; }
        public int MembershipId { get; set; }
    }
}
