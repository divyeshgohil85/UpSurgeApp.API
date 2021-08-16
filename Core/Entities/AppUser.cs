using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class AppUser 
    {
        public AppUser()
        {
            IsDeleted = false;
            IsActive = true;
            IsEmailVerified = false;
            IsAccountLocked = false;
            //CreatedBy = 1;
            UserName = FirstName + " " + LastName;
            CreatedAt = DateTime.UtcNow;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter first name")]
        [StringLength(200)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter last name")]
        [StringLength(200)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter email")]
        [StringLength(400)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$"
        , ErrorMessage = "Password must have 1 Uppercase, 1 Lowercase, 1 number, 1 special character and atleast have 6 characters")]
        public string Password { get; set; }

        public bool IsAgreeToNotify { get; set; }

        public string Promocode { get; set; }

        public string ProfilePictureUrl { get; set; }

        public bool IsAgreeToTerms { get; set; }

        [Required]
        public string UserName { get; set; }

        public Int64  Mobile { get; set; }

        public Nullable<System.DateTime> PasswordChangeDate { get; set; }
              
        [Required]
        public System.DateTime CreatedAt { get; set; }

        
        public int? CreatedBy { get; set; }

        
        public bool IsDeleted { get; set; }

        
        public bool IsActive { get; set; }


        public bool? IsAccountLocked { get; set; }


        public DateTime? AccountLockDate { get; set; }

        [Required]
        public bool IsEmailVerified { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public System.DateTime LastLoginTime { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Membership Memberships { get; set; }

        public int MembershipId { get; set; }

        public Country Country { get; set; }

        public int CountryId { get; set; }



    }
}
