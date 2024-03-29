﻿namespace HardwareAffinity.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using HardwareAffinity.Services.Data;
    using HardwareAffinity.Web.Extensions;
    using HardwareAffinity.Web.ViewModels.Categories;
    using HardwareAffinity.Web.ViewModels.Comments;
    using HardwareAffinity.Web.ViewModels.Images;
    using HardwareAffinity.Web.ViewModels.Products;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static HardwareAffinity.Common.GlobalConstants;

    public class ProductsController : BaseController
    {
        private readonly IProductsService productsService;
        private readonly IIMagesService imagesService;
        private readonly IVotesService votesService;
        private readonly ICommentsService commentsService;
        private readonly IFavoritesService favoritesService;
        private readonly ICategoriesService categoriesService;

        public ProductsController(
            IProductsService productsService,
            IIMagesService imagesService,
            IVotesService votesService,
            ICommentsService commentsService,
            IFavoritesService favoritesService,
            ICategoriesService categoriesService)
        {
            this.productsService = productsService;
            this.imagesService = imagesService;
            this.votesService = votesService;
            this.commentsService = commentsService;
            this.favoritesService = favoritesService;
            this.categoriesService = categoriesService;
        }

        public async Task<IActionResult> Details(string id)
        {
            var product = await this.productsService.GetProductAsync<ProductDetailsViewModel>(id);

            if (product == null)
            {
                this.Response.StatusCode = 404;
                return this.View("ProductNotFound");
            }

            var userId = this.User.GetId();
            product.Images = (IList<ImageInfoModel>)await this.imagesService.GetProductImagesAsync<ImageInfoModel>(id);
            product.Comments = await this.commentsService.GetProductCommentsAsync<CommentInfoModel>(id);
            product.Categories = (await this.categoriesService.GetAllCategoriesAsync<CategoryFormDataModel>()).ToList();

            foreach (var comment in product.Comments)
            {
                var commentVotes = await this.votesService.GetVotesForCommentAsync(comment.Id);
                comment.CommentPositiveVotes = commentVotes.PositiveVotes;
                comment.CommentNegativeVotes = commentVotes.NegativeVotes;
            }

            var voteInfo = await this.votesService.GetAverageRateAsync(id);
            product.AverageRate = voteInfo.Average;
            product.CountVotes = voteInfo.Count;
            product.Comment = new CommentInputModel();
            product.Comment.ProductId = id;

            product.StarsClass = await this.votesService.HasUserVotedAsync(id, userId)
                ? "disabled-stars"
                : "active-stars";

            var favoriteId = await this.favoritesService.CreateFavoriteAsync(userId);
            product.IsAddedToFavorites = await
                this.favoritesService.CheckIfFavoriteIsAddedAsync(id, favoriteId) ? true : false;

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
