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

    public class CategoriesServiceTests
    {
        private readonly ApplicationDbContext db;
        private readonly IRepository<Category> categoriesRepository;
        private readonly ICategoriesService categoriesService;

        public CategoriesServiceTests()
        {
            this.InitializeMapper();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            this.db = new ApplicationDbContext(options.Options);
            this.categoriesRepository = new EfRepository<Category>(this.db);
            this.categoriesService = new CategoriesService(this.categoriesRepository);
        }

        [Fact]
        public async Task TestIfServiceAddCategoriesAndGetThem()
        {
            var result = await this.categoriesService.CreateCategoryAsync("TV", "Sample description");
            var result2 = await this.categoriesService.CreateCategoryAsync("Audio", "Sample description");

            var categories = await this.categoriesService.GetAllCategoriesAsync<CategoryViewModel>();

            Assert.Equal(2, categories.Count());
        }

        private void InitializeMapper()
            => AutoMapperConfig.RegisterMappings(typeof(CommentsServiceTests).Assembly);
    }
}
