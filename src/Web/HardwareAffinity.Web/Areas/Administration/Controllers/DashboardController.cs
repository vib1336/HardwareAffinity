namespace HardwareAffinity.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using HardwareAffinity.Services.Data;
    using HardwareAffinity.Web.Extensions;
    using HardwareAffinity.Web.ViewModels.Categories;
    using HardwareAffinity.Web.ViewModels.Products;
    using Microsoft.AspNetCore.Mvc;

    using static HardwareAffinity.Common.GlobalConstants;

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

            var userId = this.User.GetId();

            var categoryName = await this.categoriesService.GetCategoryNameAsync(inputModel.CategoryId);

            var productId = await this.productsService.CreateProductAsync(
                inputModel.Title,
                inputModel.Description,
                inputModel.Price,
                inputModel.CategoryId,
                userId,
                inputModel.Images);

            this.TempData["InfoMessage2"] = string.Format(ProductPosted, categoryName);

            return this.Redirect("/");
        }
    }
}
