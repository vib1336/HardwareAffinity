namespace HardwareAffinity.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICartsService
    {
        Task<IEnumerable<T>> GetMyCartProductsAsync<T>(int cartId);

        Task AddProductToCartAsync(string productId, int cartId);

        Task<int> CreateCartAsync(string userId);
    }
}
