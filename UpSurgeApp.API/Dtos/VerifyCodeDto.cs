using System.ComponentModel.DataAnnotations;

namespace UpSurgeApp.API.Dtos
{
    public class VerifyCodeDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Range(1000, 9999)]
        public int Code { get; set; }
    }
}
