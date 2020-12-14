namespace HardwareAffinity.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CartProduct
    {
        [Required]
        public string ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int CartId { get; set; }

        public virtual Cart Cart { get; set; }
    }
}
