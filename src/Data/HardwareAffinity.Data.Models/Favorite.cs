namespace HardwareAffinity.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using HardwareAffinity.Data.Common.Models;

    public class Favorite : BaseDeletableModel<int>
    {
        public Favorite()
        {
            this.FavoriteProducts = new HashSet<FavoriteProduct>();
        }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<FavoriteProduct> FavoriteProducts { get; set; }
    }
}
