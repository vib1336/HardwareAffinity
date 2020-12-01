namespace HardwareAffinity.Web.ViewModels.Comments
{
    using System;

    using HardwareAffinity.Data.Models;
    using HardwareAffinity.Services.Mapping;

    public class CommentInfoModel : IMapFrom<Comment>
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Content { get; set; }

        public string UserUserName { get; set; }
    }
}
