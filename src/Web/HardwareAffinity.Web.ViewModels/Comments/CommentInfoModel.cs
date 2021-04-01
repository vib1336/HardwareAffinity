namespace HardwareAffinity.Web.ViewModels.Comments
{
    using System;

    using Ganss.XSS;
    using HardwareAffinity.Data.Models;
    using HardwareAffinity.Services.Mapping;

    public class CommentInfoModel : IMapFrom<Comment>
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Content { get; set; }

        public string UserUserName { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);

        public bool IsDeleted { get; set; }
    }
}
