using Microsoft.AspNetCore.Mvc;
using MortgageCalculator.Core.Interfaces;
using MortgageCalculator.Core.Documents;

namespace MortgageCalculator.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsefulLinksController : ControllerBase
    {
        private readonly IMongoRepository<UsefulLink> _linkRepo;

        public UsefulLinksController(IMongoRepository<UsefulLink> mortgageRepo)
        {
            _linkRepo = mortgageRepo;
        }

        [HttpGet(Name = "GetUsefulLinks")]
        public async Task<IEnumerable<UsefulLink>> Get()
        {
            return await _linkRepo.GetAll();
        }

        [HttpPatch(Name = "UpdateUsefulLink")]
        public async Task Patch([FromBody] UsefulLink link)
        {
            await _linkRepo.UpdateDocument(link);
        }

        [HttpDelete(Name = "DeleteUsefulLink")]
        public async Task Delete([FromBody] UsefulLink link)
        {
            await _linkRepo.DeleteDocument(link);
        }
    }
}
