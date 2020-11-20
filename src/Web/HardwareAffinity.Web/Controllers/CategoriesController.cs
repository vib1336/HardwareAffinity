namespace HardwareAffinity.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using HardwareAffinity.Services.Data;
    using HardwareAffinity.Web.ViewModels.Categories;
    using HardwareAffinity.Web.ViewModels.Products;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static HardwareAffinity.Common.GlobalConstants;

    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService categoriesService;
        private readonly IProductsService productsService;

        public CategoriesController(ICategoriesService categoriesService, IProductsService productsService)
        {
            this.categoriesService = categoriesService;
            this.productsService = productsService;
        }

        public async Task<IActionResult> AllTVs(int page = 1)
        {
            if (page <= 0)
            {
                page = 1;
            }

            int totalTVs = await this.productsService.CountProductsFromCategoryAsync(TvCategoryId);

            int maxPage = (int)Math.Ceiling((double)totalTVs / ProductsPerPage);

            if (maxPage == 0)
            {
                // empty subcategory
            }

            var categoryViewModel = await this.categoriesService
                .GetCategoryAsync<CategoryDisplayModel>(TvCategoryId);

            categoryViewModel.AllTVs = await this.productsService
                .GetProductsForCategoryAsync<AllProductsForCategoryViewModel>(TvCategoryId);

            categoryViewModel.CurrentPage = page;
            categoryViewModel.MaxPage = maxPage;

            return this.View(categoryViewModel);
        }
    }
}
