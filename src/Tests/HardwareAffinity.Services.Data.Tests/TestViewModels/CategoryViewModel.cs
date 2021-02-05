namespace HardwareAffinity.Services.Data.Tests.TestViewModels
{
    using HardwareAffinity.Data.Models;
    using HardwareAffinity.Services.Mapping;

    public class CategoryViewModel : IMapFrom<Category>
    {
        public string Title { get; set; }
    }
}
