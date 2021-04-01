namespace HardwareAffinity.Web.Controllers
{
    using System.Threading.Tasks;

    using HardwareAffinity.Services.Data;
    using HardwareAffinity.Web.ViewModels.Comments;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static HardwareAffinity.Common.GlobalConstants;

    [ApiController]
    [Authorize(Roles = AdministratorRoleName)]
    [Route("[controller]/[action]")]
    public class CommentsApiController : ControllerBase
    {
        private readonly ICommentsService commentsService;

        public CommentsApiController(ICommentsService commentsService)
            => this.commentsService = commentsService;

        [HttpPost]
        public async Task<ActionResult<object>> DeleteComment(DeleteCommentInputModel inputModel)
        {
            var isDeleted = await this.commentsService.DeleteCommentAsync(inputModel.CommentId);

            return new { isDeleted };
        }
    }
}
