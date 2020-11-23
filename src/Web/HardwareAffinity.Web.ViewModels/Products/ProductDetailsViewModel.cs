namespace HardwareAffinity.Web.ViewModels.Products
{
    using System.Collections.Generic;

    using HardwareAffinity.Data.Models;
    using HardwareAffinity.Services.Mapping;
    using HardwareAffinity.Web.ViewModels.Images;

    public class ProductDetailsViewModel : IMapFrom<Product>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public IList<ImageInfoModel> Images { get; set; }
    }
}
