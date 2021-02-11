namespace HardwareAffinity.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using HardwareAffinity.Data.Models;
    using HardwareAffinity.Services.Data;
    using HardwareAffinity.Web.ViewModels.Products;
    using Microsoft.AspNetCore.Mvc;
    using Nest;

    using static HardwareAffinity.Common.GlobalConstants;

    public class SearchController : BaseController
    {
        private readonly IProductsService productsService;
        private readonly IElasticClient elasticClient;

        public SearchController(IProductsService productsService, IElasticClient elasticClient)
        {
            this.productsService = productsService;
            this.elasticClient = elasticClient;
        }

        public async Task<IActionResult> SearchProducts(string query)
        {
            IEnumerable<AllProductsForCategoryViewModel> foundProducts
                = new List<AllProductsForCategoryViewModel>();

            if (!string.IsNullOrEmpty(query))
            {
                // var response = await this.elasticClient.SearchAsync<Product>(s => s
                // .Query(q => q
                // .Bool(x => x
                // .Must(sh => sh
                // .Match(t => t.Field(f => f.Title.ToLower()).Query(query.ToLower()))))));

                // if (!response.IsValid || response.Documents.Count == 0)
                // {
                //     this.TempData["InfoMessage"] = NoResultsFound;
                //     return this.View(foundProducts);
                // }

                // foundProducts = response.Documents
                //    .Where(p => !p.IsDeleted)
                //    .Select(p => new AllProductsForCategoryViewModel
                //    {
                //        Id = p.Id,
                //        Title = p.Title,
                //        Price = p.Price,
                //        Description = p.Description,
                //        MainImageUrl = p.MainImageUrl,
                //    })
                //    .OrderBy(x => x.Price)
                //    .ToList();

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
