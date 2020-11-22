namespace HardwareAffinity.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using HardwareAffinity.Services.Data;
    using HardwareAffinity.Web.ViewModels.Categories;
    using HardwareAffinity.Web.ViewModels.Products;
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : AdministrationController
    {
        private readonly ICategoriesService categoriesService;
        private readonly IProductsService productsService;

        public DashboardController(ICategoriesService categoriesService, IProductsService productsService)
        {
            this.categoriesService = categoriesService;
            this.productsService = productsService;
        }

        public async Task<IActionResult> Index()
        {
            var createProductInputModel = new CreateProductInputModel
            {
                Categories = await this.categoriesService
                .GetAllCategoriesAsync<CategoryFormDataModel>(),
            };

            return this.View(createProductInputModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View("Index", inputModel);
            }

            var userId = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            var productId = await this.productsService.CreateProductAsync(
                inputModel.Title,
                inputModel.Description,
                inputModel.Price,
                inputModel.CategoryId,
                userId,
                inputModel.Images);

            return null;
        }
    }
}
