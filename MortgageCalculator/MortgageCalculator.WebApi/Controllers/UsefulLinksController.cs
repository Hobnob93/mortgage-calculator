using Microsoft.AspNetCore.Mvc;
using MortgageCalculator.Core.Interfaces;
using MortgageCalculator.Core.Models;

namespace MortgageCalculator.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsefulLinksController : ControllerBase
    {
        private readonly IUsefulLinksRepository _mortgageRepo;

        public UsefulLinksController(IUsefulLinksRepository mortgageRepo)
        {
            _mortgageRepo = mortgageRepo;
        }

        [HttpGet(Name = "GetUsefulLinks")]
        public async Task<IEnumerable<UsefulLink>> Get()
        {
            return await _mortgageRepo.GetUsefulLinks();
        }
    }
}
