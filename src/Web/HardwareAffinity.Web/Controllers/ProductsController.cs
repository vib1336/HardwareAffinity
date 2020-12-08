namespace HardwareAffinity.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using HardwareAffinity.Services.Data;
    using HardwareAffinity.Web.ViewModels.Comments;
    using HardwareAffinity.Web.ViewModels.Images;
    using HardwareAffinity.Web.ViewModels.Products;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static HardwareAffinity.Common.GlobalConstants;

    public class ProductsController : Controller
    {
        private readonly IProductsService productsService;
        private readonly IIMagesService imagesService;
        private readonly IVotesService votesService;
        private readonly ICommentsService commentsService;

        public ProductsController(
            IProductsService productsService,
            IIMagesService imagesService,
            IVotesService votesService,
            ICommentsService commentsService)
        {
            this.productsService = productsService;
            this.imagesService = imagesService;
            this.votesService = votesService;
            this.commentsService = commentsService;
        }

        public async Task<IActionResult> Details(string id)
        {
            var product = await this.productsService.GetProductAsync<ProductDetailsViewModel>(id);

            if (product == null)
            {
                this.Response.StatusCode = 404;
                return this.View("ProductNotFound");
            }

            product.Images = (IList<ImageInfoModel>)await this.imagesService.GetProductImagesAsync<ImageInfoModel>(id);
            product.Comments = await this.commentsService.GetProductCommentsAsync<CommentInfoModel>(id);

            var voteInfo = await this.votesService.GetAverageRateAsync(id);
            product.AverageRate = voteInfo.Average;

            return this.View(product);
        }

        [Authorize(Roles = AdministratorRoleName)]
        public async Task<IActionResult> EditProduct(string id)
        {
            var editModel = await this.productsService.GetProductAsync<EditProductViewModel>(id);

            return this.PartialView("_EditProductForm", editModel);
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult ConfirmDelete(string id)
        {
            return this.PartialView("_ConfirmDelete");
        }
    }
}
