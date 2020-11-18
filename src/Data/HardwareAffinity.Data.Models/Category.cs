namespace HardwareAffinity.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using HardwareAffinity.Data.Common.Models;

    using static HardwareAffinity.Common.GlobalConstants;

    public class Category : BaseModel<int>
    {
        public Category()
        {
            this.Products = new HashSet<Product>();
        }

        [Required]
        [StringLength(CategoryTitleMaxLength, MinimumLength = CategoryTitleMinLength)]
        public string Title { get; set; }

        [StringLength(CategoryDescriptionMaxLength)]
        public string Description { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
