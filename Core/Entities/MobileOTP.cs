using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class MobileOTP
    {
        public MobileOTP()
        {
            IsDeleted = false;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Country Country { get; set; }
        public int CountryId { get; set; }

        public int OTP { get; set; }

        public Int64 Mobile { get; set; }

        public bool IsVerified { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public System.DateTime CreatedAt { get; set; }

        public System.DateTime? UpdatedAt { get; set; }
    }
}
