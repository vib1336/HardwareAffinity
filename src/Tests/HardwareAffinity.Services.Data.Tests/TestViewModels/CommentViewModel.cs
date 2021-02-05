namespace HardwareAffinity.Services.Data.Tests.TestViewModels
{
    using HardwareAffinity.Data.Models;
    using HardwareAffinity.Services.Mapping;

    public class CommentViewModel : IMapFrom<Comment>
    {
        public string Content { get; set; }

        public string ProductId { get; set; }

        public string UserUserName { get; set; }
    }
}
