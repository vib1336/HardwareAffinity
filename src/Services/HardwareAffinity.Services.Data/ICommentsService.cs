namespace HardwareAffinity.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICommentsService
    {
        Task<IEnumerable<T>> GetProductCommentsAsync<T>(string productId);

        Task AddCommentAsync(string content, string productId, string userId, int? parentId = null);

        Task<bool> DeleteCommentAsync(int id);

        Task<bool> IsInProductIdAsync(int commentId, string productId);

        Task<bool> DoesCommentExistsAsync(int commentId);
    }
}
