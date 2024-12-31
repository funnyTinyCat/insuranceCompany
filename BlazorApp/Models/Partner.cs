using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Models
{
    public class Partner
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Address { get; set; }
        public required string PartnerNumber { get; set; }
        public string? CroatianPIN { get; set; }
        public required int PartnerTypeId { get; set; }
        public required DateTime CreatedAtUtc { get; set; }
        public required string CreateByUser { get; set; }
        public required Boolean IsForeign { get; set; }
        public string? ExternalCode { get; set; }
        public required char Gender { get; set; }

        // navigation properties
        public PartnerType? PartnerType { get; set; }
        public List<Policy> Policies { get; set; } = [];
    }
}
