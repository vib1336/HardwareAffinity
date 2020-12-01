namespace HardwareAffinity.Web.Controllers
{
    using System.Threading.Tasks;

    using HardwareAffinity.Services.Data;
    using HardwareAffinity.Web.ViewModels.Products;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
            await this.productsService.UpdateProductAsync(inputModel.Id, inputModel.Title, inputModel.Description);

            var product = await this.productsService.GetProductAsync<EditProductReturnInfo>(inputModel.Id);

            return product;
        }
    }
}
