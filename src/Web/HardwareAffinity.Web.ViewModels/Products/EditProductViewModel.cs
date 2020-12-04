namespace HardwareAffinity.Web.ViewModels.Products
{
    using System.ComponentModel.DataAnnotations;

    using HardwareAffinity.Data.Models;
    using HardwareAffinity.Services.Mapping;

    using static HardwareAffinity.Common.GlobalConstants;

    public class EditProductViewModel : IMapFrom<Product>
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(ProductTitleMaxLength, MinimumLength = ProductTitleMinLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(ProductDescriptionMaxLength, MinimumLength = ProductDescriptionMinLength)]
        public string Description { get; set; }

        [Range(ProductMinPrice, ProductMaxPrice)]
        public decimal Price { get; set; }
    }
}
