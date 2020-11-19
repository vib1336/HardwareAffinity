namespace HardwareAffinity.Web.ViewModels.Categories
{
    using HardwareAffinity.Data.Models;
    using HardwareAffinity.Services.Mapping;

    public class CategoryDisplayModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
