namespace HardwareAffinity.Web.ViewModels.Categories
{
    using System.ComponentModel.DataAnnotations;

    using static HardwareAffinity.Common.GlobalConstants;

    public class CreateCategoryInputModel
    {
        [Required]
        [StringLength(CategoryTitleMaxLength, MinimumLength = CategoryTitleMinLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(CategoryDescriptionMaxLength)]
        public string Description { get; set; }
    }
}
