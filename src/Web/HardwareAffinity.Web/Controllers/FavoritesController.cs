namespace HardwareAffinity.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using HardwareAffinity.Services.Data;
    using HardwareAffinity.Web.ViewModels.Favorites;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static HardwareAffinity.Common.GlobalConstants;

    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly IProductsService productsService;
        private readonly IFavoritesService favoritesService;

        public FavoritesController(IProductsService productsService, IFavoritesService favorites)
        {
            this.productsService = productsService;
            this.favoritesService = favorites;
        }

        public async Task<IActionResult> AddToFavorites(string id)
        {
            var userId = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var favoriteId = await this.favoritesService.CreateFavoriteAsync(userId);

            var productExists = await this.productsService.ProductExistsAsync(id);
            var productIsAlreadyAdded = await this.favoritesService.CheckIfFavoriteIsAddedAsync(id, favoriteId);

            if (!productExists || productIsAlreadyAdded)
            {
                this.TempData["InfoMessage"] = FavoritesErrorMessage;
                return this.Redirect("/");
            }

            await this.favoritesService.AddProductToFavoritesAsync(id, favoriteId);

            this.TempData["InfoMessage2"] = ProductSuccessfullyAddedToFavorites;
            return this.RedirectToAction("Details", "Products", new { id });
        }

        public async Task<IActionResult> MyFavorites()
        {
            var userId = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            var favoriteId = await this.favoritesService.CreateFavoriteAsync(userId);

            var favoriteProducts = await this.favoritesService.GetMyFavoriteProductsAsync<FavoriteProductViewModel>(favoriteId);

            return this.View(favoriteProducts);
        }
    }
}
