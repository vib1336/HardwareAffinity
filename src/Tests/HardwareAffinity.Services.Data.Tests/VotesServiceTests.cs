namespace HardwareAffinity.Services.Data.Tests
{
    using System;
    using System.Threading.Tasks;

    using HardwareAffinity.Data;
    using HardwareAffinity.Data.Common.Repositories;
    using HardwareAffinity.Data.Models;
    using HardwareAffinity.Data.Repositories;
    using HardwareAffinity.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class VotesServiceTests
    {
        private readonly ApplicationDbContext db;
        private readonly IDeletableEntityRepository<Vote> votesRepository;
        private readonly IDeletableEntityRepository<Product> productsRepository;
        private readonly IVotesService votesService;

        public VotesServiceTests()
        {
            this.InitializeMapper();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.db = new ApplicationDbContext(options.Options);
            this.votesRepository = new EfDeletableEntityRepository<Vote>(this.db);
            this.votesService = new VotesService(this.votesRepository, this.productsRepository);
        }

        [Fact]
        public async Task TestIfServiceAddComment()
        {
            await this.votesService.AddVoteAsync("1", 5, "userId");

            var vote = await this.votesRepository.All()
                .FirstOrDefaultAsync(v => v.ProductId == "1" && v.UserId == "userId");

            Assert.Equal("1", vote.ProductId);
            Assert.Equal("userId", vote.UserId);
        }

        [Fact]
        public async Task TestIfUserHasAlreadyVoted()
        {
            await this.votesService.AddVoteAsync("2", 3, "customUserId");

            var hasUserVoted = await this.votesService.HasUserVotedAsync("2", "customUserId");
            var hasUserVoted2 = await this.votesService.HasUserVotedAsync("2", "someOtherId");

            Assert.True(hasUserVoted);
            Assert.False(hasUserVoted2);
        }

        [Fact]
        public async Task TestIfServiceReturnsRightAverageRate()
        {
            await this.votesService.AddVoteAsync("2", 3, "customUserId");
            await this.votesService.AddVoteAsync("2", 4, "someOtherId");

            var averageRate = await this.votesService.GetAverageRateAsync("2");

            Assert.Equal(3.5, averageRate.Average);
        }

        private void InitializeMapper()
            => AutoMapperConfig.RegisterMappings(typeof(VotesServiceTests).Assembly);
    }
}
