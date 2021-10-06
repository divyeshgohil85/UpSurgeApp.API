using System.ComponentModel.DataAnnotations;

namespace UpSurgeApp.API.Dtos
{
    public class UserMembershipDto
    {
        [Required(ErrorMessage = "Please enter email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Membership value must be between {1} and {2}.")]
        [Range(0, 3)]
        public int? MembershipId { get; set; }
    }
}
