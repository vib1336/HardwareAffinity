namespace HardwareAffinity.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICategoriesService
    {
        Task<bool> CreateCategoryAsync(string title, string description);

        Task<IEnumerable<T>> GetAllCategoriesAsync<T>();

        Task<T> GetCategoryAsync<T>(int id);

        Task<string> GetCategoryNameAsync(int id);

        Task<bool> CategoryExistsAsync(int id);
    }
}
