namespace HardwareAffinity.Web.Controllers
{
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

        public async Task<IActionResult> AllTVs()
        {
            var categoryViewModel = await this.categoriesService
                .GetCategoryAsync<CategoryDisplayModel>(TvCategoryId);

            categoryViewModel.AllTVs = await this.productsService
                .GetProductsForCategoryAsync<AllProductsForCategoryViewModel>(TvCategoryId);

            return this.View(categoryViewModel);
        }
    }
}
