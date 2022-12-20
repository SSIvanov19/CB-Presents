using CBPresents.Services.Contracts;
using CBPresents.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CBPresents.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LotteryController : ControllerBase
    {
        private readonly ICurrentUser currentUser;
        private readonly ILotteryService lotteryService;
        private readonly ITimeService timeService;
        
        public LotteryController(ICurrentUser currentUser, ILotteryService lotteryService, ITimeService timeService)
        {
            this.currentUser = currentUser;
            this.lotteryService = lotteryService;
            this.timeService = timeService;
        }

        [HttpPost("participate")]
        public async Task<ActionResult<LotteryEntryVM>> Participate()
        {
            var userEmail = this.currentUser.UserEmail;

            // Check if lottery is expired            
            var lotteryTime = await this.timeService.GetTime();

            if (lotteryTime < DateTime.UtcNow)
            {
                // check for winners
                return this.BadRequest("Lottery is expired");
            }

            // Check if user can participate
            // must match regex \d+(?=\@)

            // Register the new entry
            var response  = await lotteryService.Participate(userEmail);

            response.LotteryTime = lotteryTime ?? default;

            return this.Ok(response);
        }
    }
}
