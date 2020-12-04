namespace HardwareAffinity.Web.Controllers
{
    using System.Threading.Tasks;

    using HardwareAffinity.Services.Data;
    using HardwareAffinity.Web.ViewModels.Products;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Authorize]
    [Route("[controller]/[action]")]
    public class ProductsApiController : ControllerBase
    {
        private readonly IProductsService productsService;

        public ProductsApiController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        [HttpPost]
        public async Task<ActionResult<EditProductReturnInfo>> UpdateProduct(EditProductViewModel inputModel)
        {
            await this.productsService.UpdateProductAsync(inputModel.Id, inputModel.Title, inputModel.Description, inputModel.Price);

            var product = await this.productsService.GetProductAsync<EditProductReturnInfo>(inputModel.Id);

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<object>> DeleteProduct(DeleteInputModel inputModel)
        {
            var isDeleted = await this.productsService.DeleteProductAsync(inputModel.ProductId);

            return new { isDeleted };
        }
    }
}
