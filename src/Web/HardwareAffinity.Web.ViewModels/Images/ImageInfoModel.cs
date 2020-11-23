namespace HardwareAffinity.Web.ViewModels.Images
{
    using HardwareAffinity.Data.Models;
    using HardwareAffinity.Services.Mapping;

    public class ImageInfoModel : IMapFrom<Image>
    {
        public string ImageUrl { get; set; }
    }
}
