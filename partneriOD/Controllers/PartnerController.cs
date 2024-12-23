using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using partneriOD.Interfaces;
using partneriOD.Models;

namespace partneriOD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartnerController : ControllerBase
    {
        private readonly IPartnerRepository _partnerRepository;

        public PartnerController(IPartnerRepository partnerRepository)
        {
            _partnerRepository = partnerRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Partner>>> GetAllPartners()
        {
            var partners = await _partnerRepository.GetAllPartnersAsync();
            return Ok(partners);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Partner>>> GetPartner(int id)
        {
            var partner = await _partnerRepository.GetPartnerAsync(id);
            return Ok(partner);
        }

    }
}
