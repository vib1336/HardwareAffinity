namespace HardwareAffinity.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using HardwareAffinity.Data;
    using HardwareAffinity.Data.Common.Repositories;
    using HardwareAffinity.Data.Models;
    using HardwareAffinity.Data.Repositories;
    using HardwareAffinity.Services.Data.Tests.TestViewModels;
    using HardwareAffinity.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class CommentsServiceTests
    {
        private readonly ApplicationDbContext db;
        private readonly IDeletableEntityRepository<Comment> commentsRepository;
        private readonly IDeletableEntityRepository<CommentVote> commentVotesRepository;
        private readonly ICommentsService commentsService;

        public CommentsServiceTests()
        {
            this.InitializeMapper();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.db = new ApplicationDbContext(options.Options);
            this.commentsRepository = new EfDeletableEntityRepository<Comment>(this.db);
            this.commentVotesRepository = new EfDeletableEntityRepository<CommentVote>(this.db);
            this.commentsService = new CommentsService(this.commentsRepository, this.commentVotesRepository);
        }

        [Fact]
        public async Task TestIfServiceAddComments()
        {
            await this.commentsService.AddCommentAsync("Test content", "1", "userId");

            var comment = await this.commentsRepository.All()
                .FirstOrDefaultAsync(c => c.ProductId == "1" && c.UserId == "userId");

            Assert.Equal("Test content", comment.Content);
        }

        [Fact]
        public async Task TestIfServiceReturnCommentsForParticularProduct()
        {
            await this.commentsService.AddCommentAsync("Test content", "1", "userId");
            await this.commentsService.AddCommentAsync("Test content 2", "1", "otherUserId");
            await this.commentsService.AddCommentAsync("Test content 3", "1", "userId");

            var comments = await this.commentsService.GetProductCommentsAsync<CommentViewModel>("1");

            Assert.Equal(3, comments.Count());
        }

        private void InitializeMapper()
            => AutoMapperConfig.RegisterMappings(typeof(CommentsServiceTests).Assembly);
    }
}
