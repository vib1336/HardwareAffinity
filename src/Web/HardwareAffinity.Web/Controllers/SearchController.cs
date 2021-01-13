namespace HardwareAffinity.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using HardwareAffinity.Services.Data;
    using HardwareAffinity.Web.ViewModels.Products;
    using Microsoft.AspNetCore.Mvc;

    using static HardwareAffinity.Common.GlobalConstants;

    public class SearchController : Controller
    {
        private readonly IProductsService productsService;

        public SearchController(IProductsService productsService)
            => this.productsService = productsService;

        public async Task<IActionResult> SearchProducts(string query)
        {
            IEnumerable<AllProductsForCategoryViewModel> foundProducts
                = new List<AllProductsForCategoryViewModel>();

            if (!string.IsNullOrEmpty(query))
            {
                foundProducts = await this.productsService
                    .SearchProductsAsync<AllProductsForCategoryViewModel>(query);

                if (foundProducts.Count() == 0)
                {
                    this.TempData["InfoMessage"] = NoResultsFound;
                }
            }
            else
            {
                this.TempData["InfoMessage"] = NoResultsFound;
            }

            return this.View(foundProducts);
        }
    }
}

