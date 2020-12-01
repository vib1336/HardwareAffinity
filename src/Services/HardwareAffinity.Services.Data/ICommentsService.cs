namespace HardwareAffinity.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICommentsService
    {
        Task<IEnumerable<T>> GetProductCommentsAsync<T>(string productId);

        Task AddCommentAsync(string content, string productId, string userId);
    }
}
