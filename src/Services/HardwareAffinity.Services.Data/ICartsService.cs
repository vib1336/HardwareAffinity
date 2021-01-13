namespace HardwareAffinity.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICartsService
    {
        Task<int> GetMyCartProductsCountAsync(int cartId);

        Task<IEnumerable<T>> GetMyCartProductsAsync<T>(int cartId);

        Task AddProductToCartAsync(string productId, int cartId);

        Task<int> CreateCartAsync(string userId);

        Task RemoveFromCartAsync(string productId, int cartId);
    }
}
