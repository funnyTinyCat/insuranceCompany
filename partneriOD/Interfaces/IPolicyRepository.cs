using partneriOD.Models;

namespace partneriOD.Interfaces
{
    public interface IPolicyRepository 
    {
        Task<IEnumerable<Policy>> GetAllPoliciesAsync();
        Task<Policy> GetPolicyAsync(int id);
        Task<int> CreatePolicyAsync(Policy policy);
    }
}
