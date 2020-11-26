namespace HardwareAffinity.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using HardwareAffinity.Services.Data;
    using HardwareAffinity.Web.ViewModels.Images;
    using HardwareAffinity.Web.ViewModels.Products;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class ProductsController : Controller
    {
        private readonly IProductsService productsService;
        private readonly IIMagesService imagesService;
        private readonly IVotesService votesService;

        public ProductsController(
            IProductsService productsService,
            IIMagesService imagesService,
            IVotesService votesService)
        {
            this.productsService = productsService;
            this.imagesService = imagesService;
            this.votesService = votesService;
        }

        public async Task<IActionResult> Details(string id)
        {
            var product = await this.productsService.GetProductAsync<ProductDetailsViewModel>(id);

            if (product == null)
            {
                // error 404
            }

            product.Images = (IList<ImageInfoModel>)await this.imagesService.GetProductImagesAsync<ImageInfoModel>(id);

            var voteInfo = await this.votesService.GetAverageRateAsync(id);
            product.AverageRate = voteInfo.Average;

            return this.View(product);
        }
    }
}
