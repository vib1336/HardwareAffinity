namespace HardwareAffinity.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using HardwareAffinity.Data.Common.Repositories;
    using HardwareAffinity.Data.Models;
    using HardwareAffinity.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class CommentsService : ICommentsService
    {
        private readonly IDeletableEntityRepository<Comment> commentsRepository;

        public CommentsService(IDeletableEntityRepository<Comment> commentsRepository)
            => this.commentsRepository = commentsRepository;

        public async Task AddCommentAsync(string content, string productId, string userId)
        {
            var comment = new Comment
            {
                Content = content,
                ProductId = productId,
                UserId = userId,
            };

            await this.commentsRepository.AddAsync(comment);
            await this.commentsRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetProductCommentsAsync<T>(string productId)
            => await this.commentsRepository.All().Where(c => c.ProductId == productId)
            .To<T>()
            .ToListAsync();
    }
}
