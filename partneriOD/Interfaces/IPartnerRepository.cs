using partneriOD.Models;

namespace partneriOD.Interfaces
{
    public interface IPartnerRepository
    {
        Task<IEnumerable<Partner>> GetAllPartnersAsync();
        Task<Partner> GetPartnerAsync(int id);
        Task<int> CreatePartnerAsync(Partner partner);
    }
}
