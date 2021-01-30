namespace HardwareAffinity.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using HardwareAffinity.Data.Common.Repositories;
    using HardwareAffinity.Data.Models;
    using HardwareAffinity.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class CategoriesService : ICategoriesService
    {
        private readonly IRepository<Category> categoriesRepository;

        public CategoriesService(IRepository<Category> categoriesRepository)
            => this.categoriesRepository = categoriesRepository;

        public async Task<bool> CreateCategoryAsync(string title, string description)
        {
            var exists = this.categoriesRepository.All().Any(c => c.Title.ToLower() == title.ToLower());

            if (exists)
            {
                return false;
            }

            var category = new Category
            {
                Title = title,
                Description = description,
            };

            await this.categoriesRepository.AddAsync(category);
            await this.categoriesRepository.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<T>> GetAllCategoriesAsync<T>()
            => await this.categoriesRepository.All().To<T>().ToListAsync();

        public async Task<T> GetCategoryAsync<T>(int id)
            => await this.categoriesRepository.All()
            .Where(c => c.Id == id)
            .To<T>()
            .FirstOrDefaultAsync();

        public async Task<string> GetCategoryNameAsync(int id)
            => await this.categoriesRepository.All()
            .Where(c => c.Id == id)
            .Select(c => c.Title)
            .FirstOrDefaultAsync();
    }
}
