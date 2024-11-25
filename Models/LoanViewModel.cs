namespace AssetSentry.Models
{
    public class LoanViewModel
    {
        public LoanViewModel()
        {
            NewLoan = new Loan();
        }

        public List<Loan>? Loans { get; set; }

        public List<Device>? Devices { get; set; }

        public Loan NewLoan { get; set; }
    }
}
