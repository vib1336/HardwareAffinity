namespace HardwareAffinity.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using HardwareAffinity.Data.Common.Models;

    using static HardwareAffinity.Common.GlobalConstants;

    public class Comment : BaseDeletableModel<int>
    {
        [Required]
        [StringLength(CommentMaxLength, MinimumLength = CommentMinLength)]
        public string Content { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public string ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
