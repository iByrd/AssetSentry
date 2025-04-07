using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetSentry.Models
{
    public class Loan
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a student's name.")]
        public string Student { get; set; }

        [Required(ErrorMessage = "Please enter a start date.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Please enter a end date.")]
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

        [Required]
        [ForeignKey("Device")]
        public int DeviceId { get; set; }

        public Device? Device { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(100, ErrorMessage = "Maximum of 50 characters allowed.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
    }
}
