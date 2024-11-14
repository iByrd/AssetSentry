using System.ComponentModel.DataAnnotations;

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

        public int DeviceId { get; set; }

        public Device? Device { get; set; } = null!;
    }
}
