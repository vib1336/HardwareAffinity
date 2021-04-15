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

    public class FavoritesService : IFavoritesService
    {
        private readonly IDeletableEntityRepository<Favorite> favoritesRepository;
        private readonly IDeletableEntityRepository<FavoriteProduct> favoriteProductsRepository;
        private readonly IDeletableEntityRepository<Product> productsRepository;

        public FavoritesService(
            IDeletableEntityRepository<Favorite> favoritesRepository,
            IDeletableEntityRepository<FavoriteProduct> favoriteProductsRepository,
            IDeletableEntityRepository<Product> productsRepository)
        {
            this.favoritesRepository = favoritesRepository;
            this.favoriteProductsRepository = favoriteProductsRepository;
            this.productsRepository = productsRepository;
        }

        public async Task AddProductToFavoritesAsync(string productId, int favoriteId)
        {
            var favoriteProduct = new FavoriteProduct
            {
                ProductId = productId,
                FavoriteId = favoriteId,
            };

            await this.favoriteProductsRepository.AddAsync(favoriteProduct);
            await this.favoriteProductsRepository.SaveChangesAsync();
        }

        public async Task<int> CreateFavoriteAsync(string userId)
        {
            if (userId == null)
            {
                return 0;
            }

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
        {
            if (favoriteId == 0)
            {
                return true;
            }

            return await this.favoriteProductsRepository.All()
            .AnyAsync(fp => fp.ProductId == productId && fp.FavoriteId == favoriteId);
        }

        public async Task<bool> RemoveFromFavoritesAsync(string productId, int favoriteId)
        {
            var favoriteProduct = await this.favoriteProductsRepository.All()
                .FirstOrDefaultAsync(fp => fp.ProductId == productId && fp.FavoriteId == favoriteId);

            if (favoriteProduct != null)
            {
                this.favoriteProductsRepository.Delete(favoriteProduct);

                await this.favoriteProductsRepository.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
