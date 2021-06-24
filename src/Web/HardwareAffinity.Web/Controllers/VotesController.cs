namespace HardwareAffinity.Web.Controllers
{
    using System.Threading.Tasks;

    using HardwareAffinity.Services.Data;
    using HardwareAffinity.Web.Extensions;
    using HardwareAffinity.Web.ViewModels.Votes;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Authorize]
    public class VotesController : ControllerBase
    {
        private readonly IVotesService votesService;
        private readonly IProductsService productsService;
        private readonly ICommentsService commentsService;

        public VotesController(
            IVotesService votesService,
            IProductsService productsService,
            ICommentsService commentsService)
        {
            this.votesService = votesService;
            this.productsService = productsService;
            this.commentsService = commentsService;
        }

        [HttpPost]
        [Route("Votes/Vote")]
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

        [HttpPost]
        [Route("Votes/AddVoteForComment")]
        public async Task<ActionResult<object>> AddVoteForComment(CommentVoteInputModel inputModel)
        {
            if (!await this.commentsService.DoesCommentExistsAsync(inputModel.CommentId))
            {
                return new { SuccessfulVoting = false };
            }

            var userId = this.User.GetId();

            var successfulVoting = await this.votesService.AddVoteToCommentAsync(inputModel.CommentId, inputModel.IsUpVote, userId);

            var commentVotes = await this.votesService.GetVotesForCommentAsync(inputModel.CommentId);

            return new
            {
                PositiveVotesCount = commentVotes.PositiveVotes,
                NegativeVotesCount = commentVotes.NegativeVotes,
                SuccessfulVoting = successfulVoting,
            };
        }
    }
}
