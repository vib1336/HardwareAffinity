namespace HardwareAffinity.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using HardwareAffinity.Data.Common.Models;

    using static HardwareAffinity.Common.GlobalConstants;

    public class Product : BaseDeletableModel<string>
    {
        public Product()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Images = new HashSet<Image>();
            this.Votes = new HashSet<Vote>();
            this.Comments = new HashSet<Comment>();
            this.CartProducts = new HashSet<CartProduct>();
            this.FavoriteProducts = new HashSet<FavoriteProduct>();
        }

        [Required]
        [StringLength(ProductTitleMaxLength, MinimumLength = ProductTitleMinLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(ProductDescriptionMaxLength, MinimumLength = ProductDescriptionMinLength)]
        public string Description { get; set; }

        [Range(ProductMinPrice, ProductMaxPrice)]
        public decimal Price { get; set; }

        [Required]
        public string MainImageUrl { get; set; }

        public ICollection<Image> Images { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<CartProduct> CartProducts { get; set; }

        public virtual ICollection<FavoriteProduct> FavoriteProducts { get; set; }
    }
}
