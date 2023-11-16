using CSharpBasic.Object;
using CSharpBasic.SQLAdapter;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpBasic.BussinessService
{
    // CartService
    public class CartService
    {
        private readonly CartSqlAdapter cartAdapter;
        private readonly CartDetailSqlAdapter cartDetailSqlAdapter;

        public CartService(string connectionString)
        {
            this.cartAdapter = new CartSqlAdapter(connectionString);
            this.cartDetailSqlAdapter = new CartDetailSqlAdapter(connectionString);
        }
        

        public void AddProductToCart(Guid customerId, Guid productId, int quantity)
        {
            Cart customerCart = GetCustomerCart(customerId);

            if (customerCart == null)
            {
                customerCart = new Cart
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customerId
                };

                cartAdapter.Insert(customerCart);
            }

            CartDetail cartDetail = new CartDetail
            {
                Id = customerCart.Id,
                ProductId = productId,
                Quantity = quantity
            };
            cartAdapter.Insert(cartDetail);


            CartDetail existingDetail = GetCartDetail(customerCart.Id, productId);
            if (existingDetail != null)
            {
                existingDetail.Quantity += quantity;
                cartAdapter.Update(existingDetail);
            }
            else
            {
                cartAdapter.Insert(cartDetail);
            }

            Console.WriteLine($"Product added to cart. Cart ID: {customerCart.Id}");
        }

        public List<Cart> GetCustomerCarts()
        {
            return cartAdapter.GetData<Cart>();
        }

        public List<CartDetail> GetCartDetails(Guid Id)
        {
            // Get cart details based on cartId
            return GetCartDetailsByCartId(Id);
        }

        public void ViewCustomerCart(Guid customerId)
        {
            Cart customerCart = GetCustomerCart(customerId);

            if (customerCart != null)
            {
                Console.WriteLine($"Customer Cart for Customer ID {customerId} (Cart ID: {customerCart.Id}):");

                // Display cart details
                List<CartDetail> cartDetails = GetCartDetailsByCartId(customerCart.Id);

                Console.WriteLine("Cart Details:");
                foreach (var cartDetail in cartDetails)
                {
                    Console.WriteLine($"Product ID: {cartDetail.ProductId}, Quantity: {cartDetail.Quantity}");
                }
            }
            else
            {
                Console.WriteLine($"Customer Cart not found for Customer ID {customerId}.");
            }
        }

        public void RemoveProductFromCart(Guid customerId, Guid productId)
        {
            // Remove product from the cart
            CartDetail cartDetail = GetCartDetail(customerId, productId);
            if (cartDetail != null)
            {
                cartAdapter.Delete<CartDetail>(cartDetail.Id);
                Console.WriteLine($"Product removed from cart. Customer ID: {customerId}, Product ID: {productId}");
            }
            else
            {
                Console.WriteLine($"Product not found in the cart. Customer ID: {customerId}, Product ID: {productId}");
            }
        }

        private Cart GetCustomerCart(Guid customerId)
        {
            // Get customer cart from the database
            List<Cart> customerCarts = cartAdapter.GetData<Cart>();
            return customerCarts.FirstOrDefault(c => c.CustomerId == customerId);
        }

        private List<CartDetail> GetCartDetailsByCartId(Guid Id)
        {
            // Get cart details based on cartId
            return cartAdapter.GetData<CartDetail>().Where(cd => cd.Id == Id).ToList();
        }

        private CartDetail GetCartDetail(Guid Id, Guid productId)
        {
            var cartDetails = cartAdapter.GetData<CartDetail>();

            if (cartDetails != null && cartDetails.Any())
            {
                var cartDetail = cartDetails.FirstOrDefault(cd => cd.Id == Id && cd.ProductId == productId);

                if (cartDetail != null)
                {
                    Console.WriteLine($"Found CartDetail: CartId = {cartDetail.Id}, ProductId = {cartDetail.ProductId}, Quantity = {cartDetail.Quantity}");

                    var product = GetProductById(productId);
                    if (product != null)
                    {
                        decimal totalValue = cartDetail.Quantity * product.Price;
                        Console.WriteLine($"Total Value: {totalValue}");
                    }

                    return cartDetail;
                }
                else
                {
                    Console.WriteLine($"CartDetail not found for Id = {Id} and ProductId = {productId}");
                }
            }
            else
            {
                Console.WriteLine("cartDetails is null or empty.");
            }

            return null;
        }


        private Product GetProductById(Guid productId)
        {
            
            var products = cartAdapter.GetData<Product>();
            return products.FirstOrDefault(p => p.Id == productId);
        }



    }
}
