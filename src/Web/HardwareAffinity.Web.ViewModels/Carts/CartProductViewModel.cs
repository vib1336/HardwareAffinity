namespace HardwareAffinity.Web.ViewModels.Carts
{
    using HardwareAffinity.Data.Models;
    using HardwareAffinity.Services.Mapping;

    public class CartProductViewModel : IMapFrom<Product>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string MainImageUrl { get; set; }

        public string ShortDescription
            => this.Description.Length > 20 ? this.Description.Substring(0, 20) + "..." : this.Description;
    }
}
