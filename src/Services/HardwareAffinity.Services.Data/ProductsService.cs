namespace HardwareAffinity.Services.Data
{
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

    public class ProductsService : IProductsService
    {
        private readonly IDeletableEntityRepository<Product> productsRepository;
        private readonly IDeletableEntityRepository<Image> imagesRepository;
        private readonly Cloudinary cloudinary;

        public ProductsService(
            IDeletableEntityRepository<Product> productsRepository,
            IDeletableEntityRepository<Image> imagesRepository,
            Cloudinary cloudinary)
        {
            this.productsRepository = productsRepository;
            this.imagesRepository = imagesRepository;
            this.cloudinary = cloudinary;
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

        public async Task<IEnumerable<T>> GetProductsForCategoryAsync<T>(int categoryId)
            => await this.productsRepository.All()
            .Where(p => p.CategoryId == categoryId)
            .To<T>()
            .ToListAsync();

        public async Task<T> GetProductAsync<T>(string id)
            => await this.productsRepository.All()
            .Where(p => p.Id == id)
            .To<T>()
            .FirstOrDefaultAsync();
    }
}
