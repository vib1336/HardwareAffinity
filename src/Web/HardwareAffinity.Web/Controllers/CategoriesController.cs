namespace HardwareAffinity.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using HardwareAffinity.Services.Data;
    using HardwareAffinity.Web.ViewModels.Categories;
    using HardwareAffinity.Web.ViewModels.Products;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static HardwareAffinity.Common.GlobalConstants;

    public class CategoriesController : Controller
    {
        private readonly ICategoriesService categoriesService;
        private readonly IProductsService productsService;

        public CategoriesController(ICategoriesService categoriesService, IProductsService productsService)
        {
            this.categoriesService = categoriesService;
            this.productsService = productsService;
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult AddCategory()
        {
            return this.View(new CreateCategoryInputModel());
        }

        [Authorize(Roles = AdministratorRoleName)]
        [HttpPost]
        public async Task<IActionResult> AddCategory(CreateCategoryInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.categoriesService.CreateCategoryAsync(inputModel.Title, inputModel.Description);

            this.TempData["InfoMessage2"] = SuccessfullyAddedCategory;
            return this.Redirect("/");
        }

        public async Task<IActionResult> All(int categoryId, int page = 1)
        {
            int totalCategoryProducts = await this.productsService.CountProductsFromCategoryAsync(categoryId);

            int maxPage = (int)Math.Ceiling((double)totalCategoryProducts / ProductsPerPage);

            if (maxPage == 0)
            {
                return this.View("EmptyCategory");
            }

            if (page <= 0)
            {
                page = 1;
            }

            if (page > maxPage)
            {
                page = maxPage;
            }

            var categoryViewModel = await this.categoriesService
                .GetCategoryAsync<CategoryDisplayModel>(categoryId);

            categoryViewModel.AllProducts = await this.productsService
                .OrderProductsByPriceAsync<AllProductsForCategoryViewModel>(categoryId);
            categoryViewModel.AreOrderedByPrice = true;

            categoryViewModel.Total = totalCategoryProducts;
            categoryViewModel.CurrentPage = page;
            categoryViewModel.MaxPage = maxPage;

            return this.View(categoryViewModel);
        }

        public async Task<IActionResult> OrderByPrice(int page, int categoryId)
        {
            var productOrdering = await this.ProductsOrdering(page, categoryId, isOrderAscending: true, orderByPrice: true);

            if (productOrdering.IsCategoryEmpty)
            {
                return this.View("EmptyCategory");
            }

            return this.View(productOrdering.ViewName, productOrdering.CategoryViewModel);
        }

        public async Task<IActionResult> OrderByPriceDescending(int page, int categoryId)
        {
            var productOrdering = await this.ProductsOrdering(page, categoryId, isOrderAscending: false, orderByPrice: true);

            if (productOrdering.IsCategoryEmpty)
            {
                return this.View("EmptyCategory");
            }

            return this.View(productOrdering.ViewName, productOrdering.CategoryViewModel);
        }

        public async Task<IActionResult> OrderByName(int page, int categoryId)
        {
            var productOrdering = await this.ProductsOrdering(page, categoryId, isOrderAscending: true, orderByPrice: false);

            if (productOrdering.IsCategoryEmpty)
            {
                return this.View("EmptyCategory");
            }

            return this.View(productOrdering.ViewName, productOrdering.CategoryViewModel);
        }

        public async Task<IActionResult> OrderByNameDescending(int page, int categoryId)
        {
            var productOrdering = await this.ProductsOrdering(page, categoryId, isOrderAscending: false, orderByPrice: false);

            if (productOrdering.IsCategoryEmpty)
            {
                return this.View("EmptyCategory");
            }

            return this.View(productOrdering.ViewName, productOrdering.CategoryViewModel);
        }

        // helper method with common sorting logic
        private async Task<(string ViewName, CategoryDisplayModel CategoryViewModel, bool IsCategoryEmpty)> ProductsOrdering(
            int page,
            int categoryId,
            bool isOrderAscending,
            bool orderByPrice)
        {
            if (page <= 0)
            {
                page = 1;
            }

            int totalProducts = await this.productsService.CountProductsFromCategoryAsync(categoryId);

            int maxPage = (int)Math.Ceiling((double)totalProducts / ProductsPerPage);

            if (maxPage == 0)
            {
                return (string.Empty, null, true);
            }

            var categoryViewModel = await this.categoriesService
                .GetCategoryAsync<CategoryDisplayModel>(categoryId);

            if (orderByPrice)
            {
                if (isOrderAscending)
                {
                    categoryViewModel.AllProducts =
                        await this.productsService
                        .OrderProductsByPriceAsync<AllProductsForCategoryViewModel>(categoryId);
                    categoryViewModel.AreOrderedByPrice = isOrderAscending;
                }
                else
                {
                    categoryViewModel.AllProducts =
                        await this.productsService
                        .OrderProductsByPriceDescendingAsync<AllProductsForCategoryViewModel>(categoryId);
                    categoryViewModel.AreOrderedByPrice = isOrderAscending;
                }
            }
            else
            {
                if (isOrderAscending)
                {
                    categoryViewModel.AllProducts =
                        await this.productsService
                        .OrderProductsByNameAsync<AllProductsForCategoryViewModel>(categoryId);
                    categoryViewModel.AreOrderedByName = isOrderAscending;
                }
                else
                {
                    categoryViewModel.AllProducts =
                        await this.productsService
                        .OrderProductsByNameDescendingAsync<AllProductsForCategoryViewModel>(categoryId);
                    categoryViewModel.AreOrderedByName = isOrderAscending;
                }
            }

            categoryViewModel.Total = totalProducts;
            categoryViewModel.CurrentPage = page;
            categoryViewModel.MaxPage = maxPage;

            return (CategoriesView, categoryViewModel, false);
        }
    }
}
