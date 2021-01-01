namespace HardwareAffinity.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFavoritesService
    {
        Task<IEnumerable<T>> GetMyFavoriteProductsAsync<T>(int favoriteId);

        Task AddProductToFavoritesAsync(string productId, int favoriteId);

        Task<int> CreateFavoriteAsync(string userId);

        Task<bool> CheckIfFavoriteIsAddedAsync(string productId, int favoriteId);
    }
}
