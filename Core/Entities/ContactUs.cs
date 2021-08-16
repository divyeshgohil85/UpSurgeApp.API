using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Entities
{
    public class ContactUs
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter email")]
        [StringLength(400)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter Topic")]
        [StringLength(400)]
        public string Topic { get; set; }
        [Required(ErrorMessage = "Please enter Message")]
        [StringLength(10000)]
        public string Message { get; set; }
    }
}
