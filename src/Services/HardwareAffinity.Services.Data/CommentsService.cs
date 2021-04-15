namespace HardwareAffinity.Services.Data
{
    using System;
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

        public async Task AddCommentAsync(string content, string productId, string userId, int? parentId = null)
        {
            var comment = new Comment
            {
                Content = content,
                ProductId = productId,
                UserId = userId,
                ParentId = parentId,
            };

            await this.commentsRepository.AddAsync(comment);
            await this.commentsRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetProductCommentsAsync<T>(string productId)
            => await this.commentsRepository.AllWithDeleted().Where(c => c.ProductId == productId)
            .To<T>()
            .ToListAsync();

        public async Task<bool> DeleteCommentAsync(int id)
        {
            var comment = await this.commentsRepository.All().FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null)
            {
                return false;
            }

            this.commentsRepository.Delete(comment);

            await this.commentsRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsInProductIdAsync(int commentId, string productId)
        {
            var commentProductId = await this.commentsRepository.All()
                .Where(c => c.Id == commentId)
                .Select(c => c.ProductId)
                .FirstOrDefaultAsync();
            return commentProductId == productId;
        }
    }
}
