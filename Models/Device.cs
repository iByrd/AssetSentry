using System.ComponentModel.DataAnnotations;

namespace AssetSentry.Models
{
    public class Device
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a description.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please select a status.")]
        public string StatusId { get; set; }

        public Status? Status { get; set; }

        public Loan? Loan { get; set; }
    }
}
