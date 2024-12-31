namespace BlazorApp.Models
{
    public class Policy
    {
        public int Id { get; set; }
        public required string PolicyNumber { get; set; }
        public required decimal PolicyAmount { get; set; }
    }
}
