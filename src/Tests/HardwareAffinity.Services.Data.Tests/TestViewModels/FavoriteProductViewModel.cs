namespace HardwareAffinity.Services.Data.Tests.TestViewModels
{
    using HardwareAffinity.Data.Models;
    using HardwareAffinity.Services.Mapping;

    public class FavoriteProductViewModel : IMapFrom<Product>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string MainImageUrl { get; set; }
    }
}
