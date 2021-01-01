namespace HardwareAffinity.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using HardwareAffinity.Data.Common.Repositories;
    using HardwareAffinity.Data.Models;
    using HardwareAffinity.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class FavoritesService : IFavoritesService
    {
        private readonly IDeletableEntityRepository<Favorite> favoritesRepository;
        private readonly IDeletableEntityRepository<FavoriteProduct> favoriteProductRepository;
        private readonly IDeletableEntityRepository<Product> productsRepository;

        public FavoritesService(
            IDeletableEntityRepository<Favorite> favoritesRepository,
            IDeletableEntityRepository<FavoriteProduct> favoriteProductRepository,
            IDeletableEntityRepository<Product> productsRepository)
        {
            this.favoritesRepository = favoritesRepository;
            this.favoriteProductRepository = favoriteProductRepository;
            this.productsRepository = productsRepository;
        }

        public async Task AddProductToFavoritesAsync(string productId, int favoriteId)
        {
            var favoriteProduct = new FavoriteProduct
            {
                ProductId = productId,
                FavoriteId = favoriteId,
            };

            await this.favoriteProductRepository.AddAsync(favoriteProduct);
            await this.favoriteProductRepository.SaveChangesAsync();
        }

        public async Task<int> CreateFavoriteAsync(string userId)
        {
            var userFavorite = await this.favoritesRepository.All().FirstOrDefaultAsync(f => f.UserId == userId);

            if (userFavorite != null)
            {
                return userFavorite.Id;
            }

            var favorite = new Favorite
            {
                UserId = userId,
            };

            await this.favoritesRepository.AddAsync(favorite);
            await this.favoritesRepository.SaveChangesAsync();

            return favorite.Id;
        }

        public async Task<IEnumerable<T>> GetMyFavoriteProductsAsync<T>(int favoriteId)
            => await this.productsRepository.All()
            .Where(p => p.FavoriteProducts.Any(fp => fp.FavoriteId == favoriteId))
            .To<T>()
            .ToListAsync();

        public async Task<bool> CheckIfFavoriteIsAddedAsync(string productId, int favoriteId)
            => await this.favoriteProductRepository.All()
            .AnyAsync(fp => fp.ProductId == productId && fp.FavoriteId == favoriteId);
    }
}
