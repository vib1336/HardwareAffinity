namespace HardwareAffinity.Web.ViewModels.Products
{
    using HardwareAffinity.Data.Models;
    using HardwareAffinity.Services.Mapping;

    public class EditProductReturnInfo : IMapFrom<Product>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}
