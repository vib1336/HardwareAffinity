namespace HardwareAffinity.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using HardwareAffinity.Data.Common.Models;

    public class CommentVote : BaseDeletableModel<int>
    {
        public int CommentId { get; set; }

        public virtual Comment Comment { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public CommentVoteType VoteType { get; set; }
    }
}
