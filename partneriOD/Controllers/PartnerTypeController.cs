using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using partneriOD.Interfaces;
using partneriOD.Models;
using partneriOD.Repositories;

namespace partneriOD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartnerTypeController : ControllerBase
    {
        private readonly IPartnerTypeRepository _partnerTypeRepository;

        public PartnerTypeController(IPartnerTypeRepository partnerTypeRepository)
        {
            _partnerTypeRepository = partnerTypeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<PartnerType>>> GetAllPartnerTypes()
        {
            var partnerTypes = await _partnerTypeRepository.GetAllPartnerTypesAsync();

            return Ok(partnerTypes);
        }
    }
}
