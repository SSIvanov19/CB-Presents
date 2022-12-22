using CBPresents.Services.Contracts;
using CBPresents.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CBPresents.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly ILotteryService lotteryService;
        private readonly ITimeService timeService;
        private readonly IJobsService jobsService;
        private readonly INumberOfWinnersService numberOfWinnersService;

        public AdminController(
            ILotteryService lotteryService,
            ITimeService timeService,
            IJobsService jobsService,
            INumberOfWinnersService numberOfWinnersService)
        {
            this.lotteryService = lotteryService;
            this.timeService = timeService;
            this.jobsService = jobsService;
            this.numberOfWinnersService = numberOfWinnersService;
        }

        [HttpGet("users")]
        public async Task<ActionResult<List<User>>> GetParticipingUser()
        {
            return Ok(await lotteryService.GetParticipingUsers());
        }

        [HttpPost("updateTime")]
        public async Task<IActionResult> UpdateTimer([FromBody] DateTime dateTime)
        {
            await timeService.SetExplicitTime(dateTime);
            await jobsService.ScheduleJob();

            return Ok();
        }

        [HttpGet("getTime")]
        public async Task<ActionResult<DateTime>> GetTime()
        {
            return Ok(await timeService.GetTime());
        }

        [HttpPost("updateNumberOfWinners")]
        public async Task<IActionResult> UpdateNumberOfWinners([FromBody] int numberOfWinners)
        {
            await this.numberOfWinnersService.SetExplicitNumberOfWiinners(numberOfWinners);

            return Ok();
        }

        [HttpGet("getNumberOfWinners")]
        public async Task<ActionResult<int>> GetNumberOfWinners()
        {
            return Ok(await this.numberOfWinnersService.GetNumberOfWinners());
        }
        
        [HttpPost("pickthewinners")]
        public async Task<IActionResult> PickWinners()
        {
            var numberOfWinners = await this.numberOfWinnersService.GetNumberOfWinners();

            await lotteryService.PickWinners(numberOfWinners ?? 50);

            await timeService.SetExplicitTime(DateTime.Now);

            await jobsService.RemoveJob();

            return this.Ok();
        }
    }
}
