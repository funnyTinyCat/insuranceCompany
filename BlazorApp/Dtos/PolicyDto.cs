using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Dtos
{
    public class PolicyDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This Field is Required."),
            MinLength(10, ErrorMessage = "Range has to be between 10 and 15 characters."),
            StringLength(15, ErrorMessage = "Range has to be between 10 and 15 characters.")]
        public  string PolicyNumber { get; set; } = string.Empty;
        [Required(ErrorMessage = "This Field is Required.")]
        [RegularExpression(@"^(?=.*[1-9])\d*\.?\d*$",
            ErrorMessage = "Can not be 0.")]
        public  decimal PolicyAmount { get; set; }
    }
}
