namespace HardwareAffinity.Web.ViewModels.Products
{
    using System.Collections.Generic;

    using HardwareAffinity.Data.Models;
    using HardwareAffinity.Services.Mapping;
    using HardwareAffinity.Web.ViewModels.Comments;
    using HardwareAffinity.Web.ViewModels.Images;

    public class ProductDetailsViewModel : IMapFrom<Product>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public double AverageRate { get; set; }

        public int CountVotes { get; set; }

        public string StarsClass { get; set; }

        public CommentInputModel Comment { get; set; }

        public IList<ImageInfoModel> Images { get; set; }

        public IEnumerable<CommentInfoModel> Comments { get; set; }
    }
}
