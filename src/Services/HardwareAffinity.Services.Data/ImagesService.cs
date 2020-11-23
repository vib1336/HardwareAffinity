namespace HardwareAffinity.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using HardwareAffinity.Data.Common.Repositories;
    using HardwareAffinity.Data.Models;
    using HardwareAffinity.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class ImagesService : IIMagesService
    {
        private readonly IDeletableEntityRepository<Image> imagesRepository;

        public ImagesService(IDeletableEntityRepository<Image> imagesRepository)
        {
            this.imagesRepository = imagesRepository;
        }

        public async Task<IEnumerable<T>> GetProductImagesAsync<T>(string productId)
            => await this.imagesRepository.All()
            .Where(i => i.ProductId == productId)
            .To<T>()
            .ToListAsync();
    }
}
