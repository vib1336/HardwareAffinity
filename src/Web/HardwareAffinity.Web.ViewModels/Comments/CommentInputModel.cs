namespace HardwareAffinity.Web.ViewModels.Comments
{
    using System.ComponentModel.DataAnnotations;

    using static HardwareAffinity.Common.GlobalConstants;

    public class CommentInputModel
    {
        [Required]
        [StringLength(CommentMaxLength, MinimumLength = CommentMinLength)]
        public string Content { get; set; }

        [Required]
        public string ProductId { get; set; }
    }
}
