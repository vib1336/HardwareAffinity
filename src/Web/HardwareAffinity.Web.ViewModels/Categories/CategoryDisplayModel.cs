namespace HardwareAffinity.Web.ViewModels.Categories
{
    using System;
    using System.Collections.Generic;

    using HardwareAffinity.Data.Models;
    using HardwareAffinity.Services.Mapping;
    using HardwareAffinity.Web.ViewModels.Products;

    using static HardwareAffinity.Common.GlobalConstants;

    public class CategoryDisplayModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Total { get; set; }

        public int CurrentPage { get; set; }

        public int MaxPage { get; set; }

        public bool PreviousDisabled => this.CurrentPage == 1;

        public bool NextDisabled
        {
            get
            {
                double maxPage = Math.Ceiling(((double)this.Total) / ProductsPerPage);

                return this.CurrentPage == maxPage;
            }
        }

        public bool AreOrderedByPrice { get; set; }

        public bool AreOrderedByName { get; set; }

        public IEnumerable<AllProductsForCategoryViewModel> AllProducts { get; set; }
    }
}
