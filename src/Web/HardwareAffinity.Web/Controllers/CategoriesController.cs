namespace HardwareAffinity.Web.Controllers
{
    using System.Threading.Tasks;

    using HardwareAffinity.Services.Data;
    using HardwareAffinity.Web.ViewModels.Products;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static HardwareAffinity.Common.GlobalConstants;

    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly IProductsService productsService;

        public CategoriesController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        public async Task<IActionResult> Television()
        {
            var televisionProducts = await this.productsService
                .GetProductsForCategoryAsync<AllProductsForCategoryViewModel>(TvCategoryId);

            return this.View(televisionProducts);
        }
    }
}
