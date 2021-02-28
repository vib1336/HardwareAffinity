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

    public class FavoritesServiceTests
    {
        private readonly ApplicationDbContext db;
        private readonly IDeletableEntityRepository<Favorite> favoritesRepository;
        private readonly IDeletableEntityRepository<FavoriteProduct> favoriteProductsRepository;
        private readonly IDeletableEntityRepository<Product> productsRepository;
        private readonly IFavoritesService favoritesService;

        public FavoritesServiceTests()
        {
            this.InitializeMapper();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            this.db = new ApplicationDbContext(options.Options);
            this.favoritesRepository = new EfDeletableEntityRepository<Favorite>(this.db);
            this.favoriteProductsRepository = new EfDeletableEntityRepository<FavoriteProduct>(this.db);
            this.productsRepository = new EfDeletableEntityRepository<Product>(this.db);
            this.favoritesService = new FavoritesService(
                this.favoritesRepository,
                this.favoriteProductsRepository,
                this.productsRepository);
        }

        [Fact]
        public async Task TestIfServiceCreatesFavorites()
        {
            var favoriteId = await this.favoritesService.CreateFavoriteAsync("myUserId");

            Assert.Equal(1, favoriteId);
        }

        [Fact]
        public async Task TestIfServiceGetMyFavoriteProducts()
        {
            var favoriteId = await this.favoritesService.CreateFavoriteAsync("myUserId");

            await this.favoritesService.AddProductToFavoritesAsync("1", favoriteId);
            await this.favoritesService.AddProductToFavoritesAsync("2", favoriteId);

            var favoriteProducts = await this.favoritesService.GetMyFavoriteProductsAsync<FavoriteProductViewModel>(favoriteId);

            Assert.Equal(2, favoriteProducts.Count());
        }

        [Fact]
        public async Task TestIfServiceRemovesProductFromFavorites()
        {
            var favoriteId = await this.favoritesService.CreateFavoriteAsync("myUserId");

            await this.favoritesService.AddProductToFavoritesAsync("1", favoriteId);
            await this.favoritesService.AddProductToFavoritesAsync("2", favoriteId);

            var isSuccessful = await this.favoritesService.RemoveFromFavoritesAsync("2", favoriteId);

            Assert.True(isSuccessful);
        }

        private void InitializeMapper()
            => AutoMapperConfig.RegisterMappings(typeof(FavoritesServiceTests).Assembly);
    }
}
