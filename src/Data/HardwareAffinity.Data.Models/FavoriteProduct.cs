namespace HardwareAffinity.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using HardwareAffinity.Data.Common.Models;

    public class FavoriteProduct : BaseDeletableModel<int>
    {
        [Required]
        public string ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int FavoriteId { get; set; }

        public virtual Favorite Favorite { get; set; }
    }
}
