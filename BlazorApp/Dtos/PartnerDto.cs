using BlazorApp.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Dtos
{
    public class PartnerDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="This Field is Required."),
             MinLength(2, ErrorMessage = "Range has to be between 2 and 255 characters."),
            StringLength(255, ErrorMessage = "Range has to be between 2 and 255 characters.")]
        public  string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = "This Field is Required."),
            MinLength(2,ErrorMessage = "Range has to be between 2 and 255 characters."),
            StringLength(255, ErrorMessage = "Range has to be between 2 and 255 characters.")]
        public string LastName { get; set; } = string.Empty;
        [Required(ErrorMessage = "This Field is Required."),
            MinLength(2, ErrorMessage = "Range has to be between 2 and 255 characters."),
            StringLength(255, ErrorMessage = "Range has to be between 2 and 255 characters.")]
        public string Address { get; set; } = string.Empty;
        [Required(ErrorMessage = "This Field is Required."),
            MinLength(20, ErrorMessage = "It has to be precisely 20 digits."),
            StringLength(20, ErrorMessage = "It has to be precisely 20 digits.")]
        public string PartnerNumber { get; set; } = string.Empty;
        [MinLength(11, ErrorMessage = "It has to be precisely 11 characters."),
            StringLength(11, ErrorMessage = "It has to be precisely 11 characters.")]
        public string? CroatianPIN { get; set; }
        [Required(ErrorMessage = "This Field is Required."), // check this?
        // ^(?=.*[1-9])\d*\.?\d*$
        RegularExpression(@"^(?=.*[1-9])\d*\.?\d*$", 
            ErrorMessage = "Should be selected 'Personal' or 'Legal'.")]
        public int PartnerTypeId { get; set; }
        public  DateTime CreatedAtUtc { get; set; }
        //        [RegularExpression(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])", 
        //            [ErrorMessage = "The Email field is not a valid e-mail address."),       
        [DisplayName("Email")]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$",
            ErrorMessage = "Invalid email format.")]
        [StringLength(255, ErrorMessage = "It should be within 255 characters."),
            Required(ErrorMessage = "This Field is Required.")]
        public string CreateByUser { get; set; }
        [Required(ErrorMessage = "This Field is Required.")]
        public  Boolean IsForeign { get; set; }
        [MinLength(10, ErrorMessage = "It should be between 10 and 20 characters."),
            StringLength(20, ErrorMessage = "It should be between 10 and 20 characters.")]
        public string? ExternalCode { get; set; }
        [Required(ErrorMessage = "This Field is Required."),
            RegularExpression(@"^[M,F,N]*$", ErrorMessage = "It should be only 'Male', 'Female', or 'Neutral'.")]
        public  char Gender { get; set; }

        // navigation properties
        public PartnerTypeDto? PartnerType { get; set; }
        public List<PolicyDto> Policies { get; set; } = [];
    }
}
