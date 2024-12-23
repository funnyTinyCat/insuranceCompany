using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using partneriOD.Interfaces;
using partneriOD.Models;

namespace partneriOD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyController : ControllerBase
    {

        private readonly IPolicyRepository _videoGameRepository;

        public PolicyController(IPolicyRepository videoGameRepository)
        {
            _videoGameRepository = videoGameRepository;
        }


        [HttpGet]
        public async Task<ActionResult<List<Policy>>> GetAllVideoGames()
        {
            var policies = await _videoGameRepository.GetAllPoliciesAsync();
            return Ok(policies);
        }
    }
}
