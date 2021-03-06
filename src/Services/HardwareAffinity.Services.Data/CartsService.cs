﻿namespace HardwareAffinity.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using HardwareAffinity.Data.Common.Repositories;
    using HardwareAffinity.Data.Models;
    using HardwareAffinity.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class CartsService : ICartsService
    {
        private readonly IDeletableEntityRepository<Cart> cartsRepository;
        private readonly IDeletableEntityRepository<CartProduct> cartProductsRepository;
        private readonly IDeletableEntityRepository<Product> productsRepository;

        public CartsService(
            IDeletableEntityRepository<Cart> cartsRepository,
            IDeletableEntityRepository<CartProduct> cartProductsRepository,
            IDeletableEntityRepository<Product> productsRepository)
        {
            this.cartsRepository = cartsRepository;
            this.cartProductsRepository = cartProductsRepository;
            this.productsRepository = productsRepository;
        }

        public async Task AddProductToCartAsync(string productId, int cartId)
        {
            var cartProduct = new CartProduct
            {
                ProductId = productId,
                CartId = cartId,
            };

            await this.cartProductsRepository.AddAsync(cartProduct);
            await this.cartProductsRepository.SaveChangesAsync();
        }

        public async Task<int> CreateCartAsync(string userId)
        {
            if (userId == null)
            {
                return 0;
            }

            var userCart = await this.cartsRepository.All().FirstOrDefaultAsync(c => c.UserId == userId);

            if (userCart != null)
            {
                return userCart.Id;
            }

            var cart = new Cart
            {
                UserId = userId,
            };

            await this.cartsRepository.AddAsync(cart);
            await this.cartsRepository.SaveChangesAsync();

            return cart.Id;
        }

        public async Task<IEnumerable<T>> GetMyCartProductsAsync<T>(int cartId)
                 => await this.productsRepository.All()
            .Where(p => p.CartProducts.Any(cp => cp.CartId == cartId))
            .To<T>()
            .ToListAsync();

        public async Task<int> GetMyCartProductsCountAsync(int cartId)
        {
            if (cartId == 0)
            {
                return 0;
            }

            return await this.productsRepository.All()
                  .Where(p => p.CartProducts.Any(cp => cp.CartId == cartId))
                  .CountAsync();
        }

        public async Task RemoveFromCartAsync(string productId, int cartId)
        {
            var cartProduct = await this.cartProductsRepository.All()
                .FirstOrDefaultAsync(cp => cp.ProductId == productId && cp.CartId == cartId);

            if (cartProduct != null)
            {
                this.cartProductsRepository.Delete(cartProduct);

                await this.cartProductsRepository.SaveChangesAsync();
            }
        }
    }
}
