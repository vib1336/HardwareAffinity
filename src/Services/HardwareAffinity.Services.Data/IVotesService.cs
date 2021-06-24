namespace HardwareAffinity.Services.Data
{
    using System.Threading.Tasks;

    public interface IVotesService
    {
        Task AddVoteAsync(string productId, int rate, string userId);

        Task<(double Average, int Count)> GetAverageRateAsync(string productId);

        Task<bool> HasUserVotedAsync(string productId, string userId);

        Task<bool> AddVoteToCommentAsync(int commentId, bool isUpVote, string userId);

        Task<(int PositiveVotes, int NegativeVotes)> GetVotesForCommentAsync(int commentId);
    }
}
