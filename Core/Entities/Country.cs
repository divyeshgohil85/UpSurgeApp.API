using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class Country : BaseEntity
    {
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Iso { get; set; }
        public string Imgpath { get; set; }
        public int PhoneCode { get; set; }
        public string CountryCode { get; set; }
        public bool IsDeleted { get; set; }
        //[DefaultValue("true")]
        public bool IsActive { get; set; }
        public Country()
        {
            IsDeleted = false;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
        }
        public DateTime CreatedAt { get; set; }
    }
}
