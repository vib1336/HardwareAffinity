namespace HardwareAffinity.Web.ViewModels.Favorites
{
    using HardwareAffinity.Data.Models;
    using HardwareAffinity.Services.Mapping;
    using HardwareAffinity.Web.ViewModels.Carts;

    public class FavoriteProductViewModel : CartProductViewModel, IMapFrom<Product>
    {
    }
}
