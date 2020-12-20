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

        public VotesService(IDeletableEntityRepository<Vote> votesRepository)
            => this.votesRepository = votesRepository;

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
    }
}
