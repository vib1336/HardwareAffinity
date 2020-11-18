namespace HardwareAffinity.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using HardwareAffinity.Data.Common.Models;

    public class Image : BaseDeletableModel<int>
    {
        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
