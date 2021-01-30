namespace HardwareAffinity.Web.Controllers
{
    using System.Threading.Tasks;

    using HardwareAffinity.Services.Data;
    using HardwareAffinity.Web.Extensions;
    using HardwareAffinity.Web.ViewModels.Carts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static HardwareAffinity.Common.GlobalConstants;

    [Authorize]
    public class CartsController : BaseController
    {
        private readonly ICartsService cartsService;
        private readonly IProductsService productsService;

        public CartsController(ICartsService cartsService, IProductsService productsService)
        {
            this.cartsService = cartsService;
            this.productsService = productsService;
        }

        public async Task<IActionResult> AddToCart(string productId)
        {
            var productExists = await this.productsService.ProductExistsAsync(productId);

            if (!productExists)
            {
                this.TempData["InfoMessage"] = ProductDoesNotExist;
                return this.View("Index");
            }

            var userId = this.User.GetId();

            // if it's needed create a cart for user and return the id, if not just return the cart id
            var cartId = await this.cartsService.CreateCartAsync(userId);

            await this.cartsService.AddProductToCartAsync(productId, cartId);

            this.TempData["InfoMessage2"] = ProductSuccessfullyAdded;
            return this.RedirectToAction("Details", "Products", new { id = productId });
        }

        public async Task<IActionResult> MyCart()
        {
            var userId = this.User.GetId();

            var cartId = await this.cartsService.CreateCartAsync(userId);

            var cartProducts = await this.cartsService.GetMyCartProductsAsync<CartProductViewModel>(cartId);

            return this.View(cartProducts);
        }

        public async Task<IActionResult> RemoveFromCart(string productId)
        {
            var userId = this.User.GetId();

            var productExists = await this.productsService.ProductExistsAsync(productId);

            if (!productExists)
            {
                this.TempData["InfoMessage"] = UnsuccessfulCartRemoval;
                return this.RedirectToAction(nameof(this.MyCart));
            }

            var cartId = await this.cartsService.CreateCartAsync(userId);

            await this.cartsService.RemoveFromCartAsync(productId, cartId);
            this.TempData["InfoMessage2"] = SuccessfulCartRemoval;

            return this.RedirectToAction(nameof(this.MyCart));
        }
    }
}
