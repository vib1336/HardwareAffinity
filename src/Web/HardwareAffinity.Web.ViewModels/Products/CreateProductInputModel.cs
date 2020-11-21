﻿namespace HardwareAffinity.Web.ViewModels.Products
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using HardwareAffinity.Web.ViewModels.Categories;
    using Microsoft.AspNetCore.Http;

    using static HardwareAffinity.Common.GlobalConstants;

    public class CreateProductInputModel
    {
        [Required]
        [StringLength(ProductTitleMaxLength, MinimumLength = ProductTitleMinLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(ProductDescriptionMaxLength, MinimumLength = ProductDescriptionMinLength)]
        public string Description { get; set; }

        [Range(ProductMinPrice, ProductMaxPrice)]
        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public ICollection<IFormFile> Images { get; set; }

        public IEnumerable<CategoryFormDataModel> Categories { get; set; }
    }
}
