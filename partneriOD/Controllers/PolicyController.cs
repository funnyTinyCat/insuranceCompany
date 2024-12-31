using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using partneriOD.Interfaces;
using partneriOD.Models;
using partneriOD.Repositories;

namespace partneriOD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyController : ControllerBase
    {

        private readonly IPolicyRepository _policyRepository;

        public PolicyController(IPolicyRepository policyRepository)
        {
            _policyRepository = policyRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Policy>>> GetAllVideoGames()
        {
            var policies = await _policyRepository.GetAllPoliciesAsync();
            return Ok(policies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Policy>>> GetPolicy(int id)
        {
            var policy = await _policyRepository.GetPolicyAsync(id);
            return Ok(policy);
        }

        [HttpPost]
        public async Task<ActionResult> CreateVideoGame(Policy policy)
        {
            if (policy == null)
                return BadRequest();

            var createdId = await _policyRepository.CreatePolicyAsync(policy);
            return CreatedAtAction(nameof(GetPolicy), new { id = createdId }, policy);
        }
    }
}
