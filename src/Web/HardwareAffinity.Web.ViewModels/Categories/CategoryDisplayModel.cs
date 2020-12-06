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

        public int CurrentPage { get; set; }

        public int MaxPage { get; set; }

        public bool AreOrderedByPrice { get; set; }

        public bool AreOrderedByName { get; set; }

        public IEnumerable<AllProductsForCategoryViewModel> AllProducts { get; set; }
    }
}
