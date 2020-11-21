﻿namespace HardwareAffinity.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using HardwareAffinity.Data.Common.Repositories;
    using HardwareAffinity.Data.Models;
    using HardwareAffinity.Services.Mapping;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class ProductsService : IProductsService
    {
        private readonly IDeletableEntityRepository<Product> productsRepository;

        public ProductsService(IDeletableEntityRepository<Product> productsRepository)
        {
            this.productsRepository = productsRepository;
        }

        public async Task<int> CountProductsFromCategoryAsync(int categoryId)
            => await this.productsRepository.All().CountAsync(p => p.CategoryId == categoryId);

        public Task<int> CreateProduct(string title, string description, decimal price, int categoryId, string userId, IEnumerable<IFormFile> images)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetProductsForCategoryAsync<T>(int categoryId)
            => await this.productsRepository.All()
            .Where(p => p.CategoryId == categoryId)
            .To<T>()
            .ToListAsync();

    }
}
