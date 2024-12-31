using System.ComponentModel.DataAnnotations;

namespace partneriOD.Models
{
    public class Policy
    {
        public int Id { get; set; }
        [MinLength(10, ErrorMessage = "Range has to be between 10 and 15 characters."),
            StringLength(15, ErrorMessage = "Range has to be between 10 and 15 characters.")]
        public required string PolicyNumber { get; set; }
      //  [RegularExpression(@"^(?=.*[1-9])\d*\.?\d*$", ErrorMessage = "Can not be 0.")]
        public required decimal PolicyAmount { get; set; }
    }
}
