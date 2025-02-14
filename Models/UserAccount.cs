using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace AssetSentry.Models
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class UserAccount
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(50, ErrorMessage = "Maximum of 50 characters allowed.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(50, ErrorMessage = "Maximum of 50 characters allowed.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(100, ErrorMessage = "Maximum of 50 characters allowed.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please choose if admin or not.")]
        public bool IsAdmin { get; set; }
    }
}
