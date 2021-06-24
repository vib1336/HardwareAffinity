namespace HardwareAffinity.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using HardwareAffinity.Data.Common.Repositories;
    using HardwareAffinity.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class VotesService : IVotesService
    {
        private readonly IDeletableEntityRepository<Vote> votesRepository;
        private readonly IDeletableEntityRepository<CommentVote> commentsVotesRepository;

        public VotesService(
            IDeletableEntityRepository<Vote> votesRepository,
            IDeletableEntityRepository<CommentVote> commentsVotesRepository)
        {
            this.votesRepository = votesRepository;
            this.commentsVotesRepository = commentsVotesRepository;
        }

        public async Task AddVoteAsync(string productId, int rate, string userId)
        {
            var vote = await this.votesRepository.All()
                .FirstOrDefaultAsync(v => v.ProductId == productId && v.UserId == userId);

            if (vote == null)
            {
                vote = new Vote
                {
                    ProductId = productId,
                    UserId = userId,
                    VoteType = rate switch
                    {
                        1 => VoteType.Poor,
                        2 => VoteType.Fair,
                        3 => VoteType.Average,
                        4 => VoteType.Good,
                        5 => VoteType.Excellent,
                        _ => VoteType.NoRate,
                    },
                };

                await this.votesRepository.AddAsync(vote);
                await this.votesRepository.SaveChangesAsync();
            }
        }

        public async Task<bool> AddVoteToCommentAsync(int commentId, bool isUpVote, string userId)
        {
            var commentVote = await this.commentsVotesRepository.All()
                .FirstOrDefaultAsync(cv => cv.CommentId == commentId && cv.UserId == userId);

            if (commentVote == null)
            {
                commentVote = new CommentVote
                {
                    CommentId = commentId,
                    VoteType = isUpVote ? CommentVoteType.UpVote : CommentVoteType.DownVote,
                    UserId = userId,
                };

                await this.commentsVotesRepository.AddAsync(commentVote);
                await this.commentsVotesRepository.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<(double Average, int Count)> GetAverageRateAsync(string productId)
        {
            var countVotes = await this.votesRepository.All().CountAsync(v => v.ProductId == productId);

            if (countVotes == 0)
            {
                return (0, 0);
            }

            var sumVotes = await this.votesRepository.All()
                .Where(v => v.ProductId == productId)
                .SumAsync(v => (int)v.VoteType);

            var average = (double)sumVotes / countVotes;

            return (average, countVotes);
        }

        public async Task<bool> HasUserVotedAsync(string productId, string userId)
        {
            if (userId == null)
            {
                return true;
            }

            return await this.votesRepository.All().AnyAsync(v => v.ProductId == productId && v.UserId == userId);
        }

        public async Task<(int PositiveVotes, int NegativeVotes)> GetVotesForCommentAsync(int commentId)
        {
            var positiveVotes = await this.commentsVotesRepository.All()
                .CountAsync(cv => cv.CommentId == commentId && cv.VoteType == CommentVoteType.UpVote);

            var negativeVotes = await this.commentsVotesRepository.All()
                .CountAsync(cv => cv.CommentId == commentId && cv.VoteType == CommentVoteType.DownVote);

            return (positiveVotes, negativeVotes);
        }
    }
}
