namespace HardwareAffinity.Web.ViewModels.Comments
{
    using System.ComponentModel.DataAnnotations;

    using static HardwareAffinity.Common.GlobalConstants;

    public class CommentInputModel
    {
        [Required(ErrorMessage = RequiredComment)]
        [StringLength(CommentMaxLength, MinimumLength = CommentMinLength)]
        public string Content { get; set; }

        public int ParentId { get; set; }

        [Required]
        public string ProductId { get; set; }
    }
}
