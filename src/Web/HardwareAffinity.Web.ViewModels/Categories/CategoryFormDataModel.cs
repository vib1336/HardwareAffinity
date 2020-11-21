namespace HardwareAffinity.Web.ViewModels.Categories
{
    using HardwareAffinity.Data.Models;
    using HardwareAffinity.Services.Mapping;

    public class CategoryFormDataModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }
}
