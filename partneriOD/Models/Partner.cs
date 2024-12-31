using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace partneriOD.Models
{
    public class Partner
    {
        public int Id { get; set; }
        [MinLength(2, ErrorMessage = "Range has to be between 2 and 255 characters."),
            StringLength(255, ErrorMessage = "Range has to be between 2 and 255 characters.")]
        public required string FirstName { get; set; }
        [MinLength(2, ErrorMessage = "Range has to be between 2 and 255 characters."),
            StringLength(255, ErrorMessage = "Range has to be between 2 and 255 characters.")]
        public required string LastName { get; set; }
        [MinLength(2, ErrorMessage = "Range has to be between 2 and 255 characters."),
            StringLength(255, ErrorMessage = "Range has to be between 2 and 255 characters.")]
        public required string Address { get; set; }
        [MinLength(20, ErrorMessage = "It has to be precisely 20 digits."),
            StringLength(20, ErrorMessage = "It has to be precisely 20 digits.")]
        public required string PartnerNumber { get; set; }
        [MinLength(11, ErrorMessage = "It has to be precisely 11 characters."),
            StringLength(11, ErrorMessage = "It has to be precisely 11 characters.")]
        public string? CroatianPIN { get; set; }
        [RegularExpression(@"^(?=.*[1-9])\d*\.?\d*$",
            ErrorMessage = "Should be selected: 'Personal' or 'Legal'.")]
        public required int PartnerTypeId { get; set; }

        public required DateTime CreatedAtUtc { get; set; }
        [DisplayName("Email")]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$",
            ErrorMessage = "Invalid email format.")]
        [StringLength(255, ErrorMessage = "It should be within 255 characters.")]
        public required string CreateByUser { get; set; }

        public required Boolean IsForeign { get; set; }
        [MinLength(10, ErrorMessage = "It should be between 10 and 20 characters."),
            StringLength(20, ErrorMessage = "It should be between 10 and 20 characters.")]
        public string? ExternalCode  { get; set; }
        [RegularExpression(@"^[M,F,N]*$", ErrorMessage = "It should be only 'Male', 'Female', or 'Neutral'.")]
        public required char Gender { get; set; }

        // navigation properties
        public PartnerType? PartnerType { get; set; }
        public List<Policy> Policies { get; set; } = [];

    }
}
