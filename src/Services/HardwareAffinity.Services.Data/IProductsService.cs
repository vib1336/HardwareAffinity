namespace HardwareAffinity.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface IProductsService
    {
        Task<IEnumerable<T>> GetProductsForCategoryAsync<T>(int categoryId);

        Task<int> CountProductsFromCategoryAsync(int categoryId);

        Task<string> CreateProductAsync(
            string title,
            string description,
            decimal price,
            int categoryId,
            string userId,
            IEnumerable<IFormFile> images);

        Task<T> GetProductAsync<T>(string id);

        Task<bool> ProductExistsAsync(string id);

        Task<bool> ProductIsDeletedAsync(string id);

        Task UpdateProductAsync(string id, string title, string description, decimal price);

        Task<bool> DeleteProductAsync(string id);
    }
}
