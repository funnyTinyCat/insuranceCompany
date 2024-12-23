using partneriOD.Models;

namespace partneriOD.Interfaces
{
    public interface IPolicyRepository 
    {
        Task<IEnumerable<Policy>> GetAllPoliciesAsync();
    }
}
