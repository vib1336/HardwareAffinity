namespace HardwareAffinity.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using HardwareAffinity.Services.Data;
    using HardwareAffinity.Web.ViewModels.Votes;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VotesController : ControllerBase
    {
        private readonly IVotesService votesService;

        public VotesController(IVotesService votesService)
            => this.votesService = votesService;

        [HttpPost]
        public async Task<ActionResult<VoteReturnInfoModel>> Vote(VoteInputModel inputModel)
        {
            var userId = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            var hasUserVotedBefore = await this.votesService.HasUserVotedAsync(inputModel.ProductId, userId);

            await this.votesService.AddVoteAsync(inputModel.ProductId, inputModel.Rate, userId);

            var voteInfo = await this.votesService.GetAverageRateAsync(inputModel.ProductId);

            return new VoteReturnInfoModel
            {
                Average = voteInfo.Average,
                Count = voteInfo.Count,
                HasUserVotedBefore = hasUserVotedBefore,
            };
        }
    }
}
