namespace HardwareAffinity.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IIMagesService
    {
        Task<IEnumerable<T>> GetProductImagesAsync<T>(string productId);
    }
}
