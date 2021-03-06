﻿namespace HardwareAffinity.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using HardwareAffinity.Services.Data;
    using HardwareAffinity.Web.Extensions;
    using HardwareAffinity.Web.ViewModels.Comments;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static HardwareAffinity.Common.GlobalConstants;

    [Authorize]
    public class CommentsController : BaseController
    {
        private readonly ICommentsService commentsService;
        private readonly IProductsService productsService;

        public CommentsController(ICommentsService commentsService, IProductsService productsService)
        {
            this.commentsService = commentsService;
            this.productsService = productsService;
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(CommentInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View("_PostCommentForm", inputModel);
            }

            var parentId = inputModel.ParentId == 0 ? (int?)null : inputModel.ParentId;

            var productExists = await this.productsService.ProductExistsAsync(inputModel.ProductId);
            var productIsDeleted = await this.productsService.ProductIsDeletedAsync(inputModel.ProductId);

            if (parentId.HasValue)
            {
                if (!await this.commentsService.IsInProductIdAsync(parentId.Value, inputModel.ProductId))
                {
                    this.TempData["InfoMessage"] = ErrorMessage;
                    return this.Redirect("/");
                }
            }

            if (!productExists || productIsDeleted)
            {
                this.TempData["InfoMessage"] = ErrorMessage;
                return this.Redirect("/");
            }

            var userId = this.User.GetId();

            await this.commentsService.AddCommentAsync(inputModel.Content, inputModel.ProductId, userId, parentId);

            this.TempData["InfoMessage2"] = PostComment;

            return this.RedirectToAction("Details", "Products", new { id = inputModel.ProductId });
        }
    }
}
