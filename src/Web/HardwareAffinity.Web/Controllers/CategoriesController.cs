namespace HardwareAffinity.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using HardwareAffinity.Services.Data;
    using HardwareAffinity.Web.ViewModels.Categories;
    using HardwareAffinity.Web.ViewModels.Products;
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

        public async Task<IActionResult> AllTVs(int page = 1)
        {
            int totalTVs = await this.productsService.CountProductsFromCategoryAsync(TvCategoryId);

            int maxPage = (int)Math.Ceiling((double)totalTVs / ProductsPerPage);

            if (maxPage == 0)
            {
                // empty subcategory view
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
                .GetCategoryAsync<CategoryDisplayModel>(TvCategoryId);

            categoryViewModel.AllProducts = await this.productsService
                .OrderProductsByPriceAsync<AllProductsForCategoryViewModel>(TvCategoryId);
            categoryViewModel.AreOrderedByPrice = true;

            categoryViewModel.Total = totalTVs;
            categoryViewModel.CurrentPage = page;
            categoryViewModel.MaxPage = maxPage;

            return this.View(categoryViewModel);
        }

        public async Task<IActionResult> OrderByPrice(int page, int categoryId)
        {
            var productOrdering = await this.ProductsOrdering(page, categoryId, isOrderAscending: true, orderByPrice: true);

            return this.View(productOrdering.ViewName, productOrdering.CategoryViewModel);
        }

        public async Task<IActionResult> OrderByPriceDescending(int page, int categoryId)
        {
            var productOrdering = await this.ProductsOrdering(page, categoryId, isOrderAscending: false, orderByPrice: true);

            return this.View(productOrdering.ViewName, productOrdering.CategoryViewModel);
        }

        public async Task<IActionResult> OrderByName(int page, int categoryId)
        {
            var productOrdering = await this.ProductsOrdering(page, categoryId, isOrderAscending: true, orderByPrice: false);

            return this.View(productOrdering.ViewName, productOrdering.CategoryViewModel);
        }

        public async Task<IActionResult> OrderByNameDescending(int page, int categoryId)
        {
            var productOrdering = await this.ProductsOrdering(page, categoryId, isOrderAscending: false, orderByPrice: false);

            return this.View(productOrdering.ViewName, productOrdering.CategoryViewModel);
        }

        // helper method with common sorting logic
        private async Task<(string ViewName, CategoryDisplayModel CategoryViewModel)> ProductsOrdering(
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
                // empty subcategory
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

            string viewName = categoryId switch
            {
                1 => "AllTVs",
                2 => "AllSmartphones",
                3 => "AllAudio",
                _ => string.Empty,
            };

            return (viewName, categoryViewModel);
        }
    }
}
