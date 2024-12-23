namespace partneriOD.Models
{
    public class PartnerPolicy
    {
        public int Id { get; set; }
        public required int PartnerId { get; set; }
        public required int PolicyId { get; set; }

        // navigation properties
        public Partner? Partner { get; set; }
        public Policy? Policy { get; set; }
    }
}
