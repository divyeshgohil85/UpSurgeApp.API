using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Entities
{
    public class SiteUser : BaseEntity
    {

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string DisplayName { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string Password { get; set; }

        public Nullable<System.DateTime> PasswordChangeDate { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public System.DateTime CreatedAt { get; set; }

        [Required]
        public Nullable<int> CreatedBy { get; set; }

        [Required]
        public Nullable<bool> IsDeleted { get; set; }

        [Required]
        public Nullable<bool> IsActive { get; set; }

        public Nullable<bool> IsAccountLocked { get; set; }


        public Nullable<System.DateTime> AccountLockDate { get; set; }

        [Required]
        public Nullable<bool> IsEmailVerified { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public System.DateTime LastLoginTime { get; set; }

        public Nullable<int> UpdatedBy { get; set; }

        public Nullable<System.DateTime> UpdatedAt { get; set; }

        public Membership Role { get; set; }

        public Guid RoleId { get; set; }

    }

    class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            base.DateTimeFormat = "yyyy-MM-dd";
        }
    }

}
