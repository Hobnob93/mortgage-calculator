using Microsoft.AspNetCore.Mvc;
using MortgageCalculator.Core.Interfaces;
using MortgageCalculator.Core.Documents;

namespace MortgageCalculator.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsefulLinksController : ControllerBase
    {
        private readonly IUsefulLinksRepository _linkRepo;

        public UsefulLinksController(IUsefulLinksRepository mortgageRepo)
        {
            _linkRepo = mortgageRepo;
        }

        [HttpGet(Name = "GetUsefulLinks")]
        public async Task<IEnumerable<UsefulLink>> Get()
        {
            return await _linkRepo.GetLinks();
        }

        [HttpPatch(Name = "UpdateUsefulLink")]
        public async Task Patch([FromBody] UsefulLink link)
        {
            await _linkRepo.UpdateLink(link);
        }

        [HttpDelete(Name = "DeleteUsefulLink")]
        public async Task Delete([FromBody] UsefulLink link)
        {
            await _linkRepo.DeleteLink(link);
        }
    }
}
