namespace HardwareAffinity.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    using HardwareAffinity.Data.Models;
    using HardwareAffinity.Services.Mapping;
    using HardwareAffinity.Web.ViewModels.Products;

    public class CategoryDisplayModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public IEnumerable<AllProductsForCategoryViewModel> AllTVs { get; set; }
    }
}
