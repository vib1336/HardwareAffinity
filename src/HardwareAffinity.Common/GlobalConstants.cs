namespace HardwareAffinity.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "HardwareAffinity";

        public const string AdministratorRoleName = "Administrator";

        public const string AdministratorUserName = "Valentin";

        public const int ProductDescriptionMaxLength = 5000;

        public const int ProductDescriptionMinLength = 15;

        public const int ProductTitleMaxLength = 150;

        public const int ProductTitleMinLength = 3;

        public const int CategoryDescriptionMaxLength = 350;

        public const int CategoryTitleMaxLength = 50;

        public const int CategoryTitleMinLength = 2;

        public const double ProductMinPrice = 0.05;

        public const double ProductMaxPrice = 100000;

        public const int ProductsPerPage = 10;

        public const int CommentMaxLength = 400;

        public const int CommentMinLength = 5;

        public const string RequiredComment = "Comment must be at least 5 symbols long and 400 at most.";

        public const string ErrorMessage = "Cannot post comment for this product.";

        public const string FavoritesErrorMessage = "Cannot add this product to favorites.";

        public const string PostComment = "Comment was successfully added.";

        public const string UnexistentProduct = "Unexistent product cannot be modified.";

        public const string InvalidProductModel = "Input product model is not valid.";

        public const int PagesToShow = 3;

        public const string NoResultsFound = "No results were found.";

        public const string ProductDoesNotExist = "Cannot add this product to cart.";

        public const string ProductSuccessfullyAdded = "Product successfully added to cart.";

        public const string ProductSuccessfullyAddedToFavorites = "Product was successfully added to favorites.";

        public const string ProductPosted = "Product was successfully added to {0} category.";

        public const string CategoriesView = "All";

        public const string SuccessfullyAddedCategory = "Category was successfully added.";

        public const string UnsuccessfulFavoritesRemoval = "Cannot remove this product from favorites.";

        public const string SuccessfulFavoritesRemoval = "Product was successfully removed from favorites.";

        public const string UnsuccessfulCartRemoval = "Cannot remove this product from cart.";

        public const string SuccessfulCartRemoval = "Product was successfully removed from cart.";
    }
}
