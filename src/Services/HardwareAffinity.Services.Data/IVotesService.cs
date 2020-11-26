namespace HardwareAffinity.Services.Data
{
    using System.Threading.Tasks;

    public interface IVotesService
    {
        Task AddVoteAsync(string productId, int rate, string userId);

        Task<(double Average, int Count)> GetAverageRateAsync(string productId);
    }
}
