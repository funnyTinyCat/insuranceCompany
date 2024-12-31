using partneriOD.Models;

namespace partneriOD.Interfaces
{
    public interface IPartnerTypeRepository
    {
        Task<IEnumerable<PartnerType>> GetAllPartnerTypesAsync();
    }
}
