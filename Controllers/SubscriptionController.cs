using Microsoft.AspNetCore.Mvc;

using ppawproject.Models;
using ppawproject.Services;

namespace ppawproject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly SubscriptionService _subscriptionService;

        public SubscriptionController(SubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpPost("can-add-marketplace")]
        public IActionResult CanAddMarketplace([FromBody] UserRequest request)
        {
            var canAdd = _subscriptionService.CanAddMarketplace(request.UserId);
            return Ok(new { canAdd });
        }
    }
}
