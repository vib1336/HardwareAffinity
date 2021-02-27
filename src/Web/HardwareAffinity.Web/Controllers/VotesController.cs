namespace HardwareAffinity.Web.Controllers
{
    using System.Threading.Tasks;

    using HardwareAffinity.Services.Data;
    using HardwareAffinity.Web.Extensions;
    using HardwareAffinity.Web.ViewModels.Votes;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VotesController : ControllerBase
    {
        private readonly IVotesService votesService;
        private readonly IProductsService productsService;

        public VotesController(
            IVotesService votesService,
            IProductsService productsService)
        {
            this.votesService = votesService;
            this.productsService = productsService;
        }

        [HttpPost]
        public async Task<ActionResult<VoteReturnInfoModel>> Vote(VoteInputModel inputModel)
        {
            if (!await this.productsService.ProductExistsAsync(inputModel.ProductId))
            {
                return new VoteReturnInfoModel
                {
                    Average = default,
                    Count = default,
                    HasUserVotedBefore = default,
                };
            }

            var userId = this.User.GetId();

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
