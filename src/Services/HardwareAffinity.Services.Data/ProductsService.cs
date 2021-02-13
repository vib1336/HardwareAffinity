namespace HardwareAffinity.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using HardwareAffinity.Data.Common.Repositories;
    using HardwareAffinity.Data.Models;
    using HardwareAffinity.Services.Mapping;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    using static HardwareAffinity.Common.GlobalConstants;

    public class ProductsService : IProductsService
    {
        private readonly IRepository<Category> categoriesRepository;
        private readonly IDeletableEntityRepository<Product> productsRepository;
        private readonly IDeletableEntityRepository<Image> imagesRepository;
        private readonly IDeletableEntityRepository<Vote> votesRepository;
        private readonly IDeletableEntityRepository<Comment> commentsRepository;
        private readonly Cloudinary cloudinary;
        private readonly ISearchService searchService;

        public ProductsService(
            IRepository<Category> categoriesRepository,
            IDeletableEntityRepository<Product> productsRepository,
            IDeletableEntityRepository<Image> imagesRepository,
            IDeletableEntityRepository<Vote> votesRepository,
            IDeletableEntityRepository<Comment> commentsRepository,
            Cloudinary cloudinary,
            ISearchService searchService)
        {
            this.categoriesRepository = categoriesRepository;
            this.productsRepository = productsRepository;
            this.imagesRepository = imagesRepository;
            this.votesRepository = votesRepository;
            this.commentsRepository = commentsRepository;
            this.cloudinary = cloudinary;
            this.searchService = searchService;
        }

        public async Task<int> CountProductsFromCategoryAsync(int categoryId)
            => await this.productsRepository.All().CountAsync(p => p.CategoryId == categoryId);

        public async Task<string> CreateProductAsync(
            string title,
            string description,
            decimal price,
            int categoryId,
            string userId,
            IEnumerable<IFormFile> images)
        {
            if (!await this.categoriesRepository.All().AnyAsync(c => c.Id == categoryId))
            {
                return null;
            }

            var product = new Product
            {
                Title = title,
                Description = description,
                Price = price,
                CategoryId = categoryId,
                UserId = userId,
                MainImageUrl = string.Empty,
            };

            await this.productsRepository.AddAsync(product);
            await this.productsRepository.SaveChangesAsync();

            var pr = this.productsRepository.All().FirstOrDefault(p => p.Id == product.Id);

            int counter = 1;

            ImageUploadResult uploadResult = null;

            if (images != null && images.Count() > 0)
            {
                foreach (var image in images)
                {
                    if (counter == 1)
                    {
                        byte[] destinationImage;

                        using var memoryStream = new MemoryStream();
                        await image.CopyToAsync(memoryStream);
                        destinationImage = memoryStream.ToArray();

                        using var destinationStream = new MemoryStream(destinationImage);

                        var uploadParams = new ImageUploadParams
                        {
                            File = new FileDescription(image.FileName, destinationStream),
                        };

                        uploadResult = await this.cloudinary.UploadAsync(uploadParams);

                        pr.MainImageUrl = uploadResult.Uri.AbsoluteUri;
                        var img = new Image
                        {
                            ImageUrl = uploadResult.Uri.AbsoluteUri,
                            ProductId = pr.Id,
                        };

                        // var response = await this.searchService.CreateIndexAsync(pr);

                        await this.imagesRepository.AddAsync(img);

                        counter++;
                    }
                    else
                    {
                        byte[] destinationImage;

                        using var memoryStream = new MemoryStream();
                        await image.CopyToAsync(memoryStream);
                        destinationImage = memoryStream.ToArray();

                        using var destinationStream = new MemoryStream(destinationImage);

                        var uploadParams = new ImageUploadParams
                        {
                            File = new FileDescription(image.FileName, destinationStream),
                        };

                        uploadResult = await this.cloudinary.UploadAsync(uploadParams);

                        var img = new Image
                        {
                            ImageUrl = uploadResult.Uri.AbsoluteUri,
                            ProductId = pr.Id,
                        };

                        await this.imagesRepository.AddAsync(img);

                        counter++;
                    }
                }

                await this.imagesRepository.SaveChangesAsync();
            }

            return product.Id;
        }

        public async Task<IEnumerable<T>> GetProductsForCategoryAsync<T>(int categoryId, int page)
            => await this.productsRepository.All()
            .Where(p => p.CategoryId == categoryId)
            .Skip((page - 1) * ProductsPerPage)
            .Take(ProductsPerPage)
            .To<T>()
            .ToListAsync();

        public async Task<T> GetProductAsync<T>(string id)
            => await this.productsRepository.All()
            .Where(p => p.Id == id)
            .To<T>()
            .FirstOrDefaultAsync();

        public async Task<bool> ProductExistsAsync(string id)
            => await this.productsRepository.All()
            .AnyAsync(p => p.Id == id);

        public async Task<bool> ProductIsDeletedAsync(string id)
            => await this.productsRepository.All()
            .Where(p => p.Id == id)
            .Select(p => p.IsDeleted)
            .FirstOrDefaultAsync();

        public async Task UpdateProductAsync(string id, string title, string description, decimal price)
        {
            var product = await this.productsRepository.All().FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return;
            }

            product.Title = title;
            product.Description = description;
            product.Price = price;

            // var response = await this.searchService.UpdateIndexAsync(product);

            await this.productsRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteProductAsync(string id)
        {
            var productToDelete = await this.productsRepository.All().FirstOrDefaultAsync(p => p.Id == id);
            var productVotes = this.votesRepository.All().Where(v => v.ProductId == id);
            var productComments = this.commentsRepository.All().Where(c => c.ProductId == id);

            if (productToDelete == null)
            {
                return false;
            }

            productToDelete.IsDeleted = true;
            productToDelete.DeletedOn = DateTime.UtcNow;

            foreach (var vote in productVotes)
            {
                vote.IsDeleted = true;
                vote.DeletedOn = DateTime.UtcNow;
            }

            foreach (var comment in productComments)
            {
                comment.IsDeleted = true;
                comment.DeletedOn = DateTime.UtcNow;
            }

            // var response = await this.searchService.DeleteIndexAsync(productToDelete);

            await this.productsRepository.SaveChangesAsync();
            await this.votesRepository.SaveChangesAsync();
            await this.commentsRepository.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<T>> OrderProductsByPriceAsync<T>(int categoryId, int page)
            => await this.productsRepository.All()
            .Where(p => p.CategoryId == categoryId)
            .Skip((page - 1) * ProductsPerPage)
            .Take(ProductsPerPage)
            .OrderBy(p => p.Price)
            .To<T>()
            .ToListAsync();

        public async Task<IEnumerable<T>> OrderProductsByPriceDescendingAsync<T>(int categoryId, int page)
            => await this.productsRepository.All()
            .Where(p => p.CategoryId == categoryId)
            .Skip((page - 1) * ProductsPerPage)
            .Take(ProductsPerPage)
            .OrderByDescending(p => p.Price)
            .To<T>()
            .ToListAsync();

        public async Task<IEnumerable<T>> OrderProductsByNameAsync<T>(int categoryId, int page)
            => await this.productsRepository.All()
            .Where(p => p.CategoryId == categoryId)
            .Skip((page - 1) * ProductsPerPage)
            .Take(ProductsPerPage)
            .OrderBy(p => p.Title)
            .To<T>()
            .ToListAsync();

        public async Task<IEnumerable<T>> OrderProductsByNameDescendingAsync<T>(int categoryId, int page)
            => await this.productsRepository.All()
            .Where(p => p.CategoryId == categoryId)
            .Skip((page - 1) * ProductsPerPage)
            .Take(ProductsPerPage)
            .OrderByDescending(p => p.Title)
            .To<T>()
            .ToListAsync();

        public async Task<IEnumerable<T>> SearchProductsAsync<T>(string query)
            => await this.productsRepository.All()
            .Where(p => p.Title.ToLower().Contains(query.ToLower())
            || p.Description.ToLower().Contains(query.ToLower()))
            .To<T>()
            .ToListAsync();
    }
}
