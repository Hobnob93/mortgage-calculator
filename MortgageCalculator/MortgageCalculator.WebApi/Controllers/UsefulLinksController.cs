using Microsoft.AspNetCore.Mvc;
using MortgageCalculator.Core.Interfaces;
using MortgageCalculator.Core.Models;

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
            Console.WriteLine($"ID: {link.Id}");
            await _linkRepo.UpdateLink(link);
        }
    }
}
