using CBPresents.Services.Contracts;
using CBPresents.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.RegularExpressions;

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
        private readonly INumberOfWinnersService numberOfWinnersService;
        
        public LotteryController(ICurrentUser currentUser, ILotteryService lotteryService, ITimeService timeService, INumberOfWinnersService numberOfWinnersService)
        {
            this.currentUser = currentUser;
            this.lotteryService = lotteryService;
            this.timeService = timeService;
            this.numberOfWinnersService = numberOfWinnersService;
        }

        [HttpPost("participate")]
        public async Task<ActionResult<LotteryEntryVM>> Participate()
        {
            var userEmail = this.currentUser.UserEmail;

            // Check if lottery is expired            
            var lotteryTime = await this.timeService.GetTime();

            if (lotteryTime < DateTime.Now)
            {
                // check for winners
                var winnerResponse = await lotteryService.CheckIfWinner(userEmail);

                winnerResponse.LotteryTime = lotteryTime ?? default; 
                
                return this.Ok(winnerResponse);
            }

            var regex = new Regex(@"\d+(?=\@)");
            var match = regex.Match(userEmail);

            if (!match.Success)
            {
                return this.BadRequest(new LotteryEntryVM()
                {
                    CanParticipate = false,
                    LotteryTime = lotteryTime ?? default,
                });
            }

            
            // Register the new entry
            var response  = await lotteryService.Participate(userEmail, HttpContext.User.Identity!.Name!);

            response.LotteryTime = lotteryTime ?? default;

            return this.Ok(response);
        }

        [HttpPost("pickthewinners")]
        [AllowAnonymous]
        public async Task<IActionResult> PickWinners()
        {
            var httpContext = HttpContext.Request;
            var authHeader = httpContext.Headers["Authorization"];

            if (authHeader != "key" || currentUser.Role != "Admin")
            {
                return this.Unauthorized();
            }

            var numberOfWinners = await numberOfWinnersService.GetNumberOfWinners();

            await lotteryService.PickWinners(numberOfWinners ?? 50);

            await timeService.SetExplicitTime(DateTime.Now);

            return this.Ok();
        }
    }
}
