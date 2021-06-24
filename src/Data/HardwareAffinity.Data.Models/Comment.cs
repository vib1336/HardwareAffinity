namespace HardwareAffinity.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using HardwareAffinity.Data.Common.Models;

    using static HardwareAffinity.Common.GlobalConstants;

    public class Comment : BaseDeletableModel<int>
    {
        public Comment()
        {
            this.CommentVotes = new HashSet<CommentVote>();
        }

        [Required]
        [StringLength(CommentMaxLength, MinimumLength = CommentMinLength)]
        public string Content { get; set; }

        public int? ParentId { get; set; }

        public virtual Comment Parent { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public string ProductId { get; set; }

        public virtual Product Product { get; set; }

        public virtual ICollection<CommentVote> CommentVotes { get; set; }
    }
}
