namespace HardwareAffinity.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using HardwareAffinity.Data.Common.Repositories;
    using HardwareAffinity.Data.Models;
    using HardwareAffinity.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> categoriesRepository;

        public CategoryService(IRepository<Category> categoriesRepository)
            => this.categoriesRepository = categoriesRepository;

        public async Task<IEnumerable<T>> GetAllCategoriesAsync<T>()
            => await this.categoriesRepository.All().To<T>().ToListAsync();
    }
}
