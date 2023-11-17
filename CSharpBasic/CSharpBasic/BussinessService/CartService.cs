using CSharpBasic.Object;
using CSharpBasic.SQLAdapter;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpBasic.BussinessService
{
    /// <summary>
    /// CartService
    /// </summary>
    public class CartService
    {
        private readonly CartSqlAdapter cartAdapter;
        private readonly CartDetailSqlAdapter cartDetailSqlAdapter;

        public CartService(string connectionString)
        {
            this.cartAdapter = new CartSqlAdapter(connectionString);
            this.cartDetailSqlAdapter = new CartDetailSqlAdapter(connectionString);
        }

        /// <summary>
        /// AddProductToCart
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
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
            cartDetailSqlAdapter.Insert(cartDetail);


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


        /// <summary>
        /// GetCustomerCarts
        /// </summary>
        /// <returns></returns>
        public List<Cart> GetCustomerCarts()
        {
            return cartAdapter.GetData<Cart>();
        }
        /// <summary>
        /// GetCartDetails
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<CartDetail> GetCartDetails(Guid Id)
        {
            // Get cart details based on cartId
            return GetCartDetailsByCartId(Id);
        }

        /// <summary>
        /// ViewCustomerCart
        /// </summary>
        /// <param name="customerId"></param>
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

        /// <summary>
        /// RemoveProductFromCart
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="productId"></param>
        public void RemoveProductFromCart(Guid customerId, Guid productId)
        {
            // Remove product from the cart
            CartDetail cartDetail = GetCartDetail(customerId, productId);

            if (cartDetail != null)
            {
                // Check if the quantity is greater than 1, then just update the quantity
                if (cartDetail.Quantity > 1)
                {
                    cartDetail.Quantity -= 1;
                    cartDetailSqlAdapter.Update(cartDetail);
                    Console.WriteLine($"Quantity decreased for Product in cart. Customer ID: {customerId}, Product ID: {productId}");
                }
                else
                {
                    // If the quantity is 1 or less, remove the entire CartDetail
                    cartDetailSqlAdapter.Delete<CartDetail>(cartDetail.Id);
                    Console.WriteLine($"Product removed from cart. Customer ID: {customerId}, Product ID: {productId}");
                }
            }
            else
            {
                Console.WriteLine($"Product not found in the cart. Customer ID: {customerId}, Product ID: {productId}");
            }
        }

        /// <summary>
        /// ClearCustomerCart
        /// </summary>
        /// <param name="customerId"></param>
        public void ClearCustomerCart(Guid customerId)
        {
            Cart customerCart = GetCustomerCart(customerId);

            if (customerCart != null)
            {
                // Xóa tất cả chi tiết giỏ hàng của khách hàng
                List<CartDetail> cartDetails = GetCartDetailsByCartId(customerCart.Id);
                foreach (var cartDetail in cartDetails)
                {
                    cartDetailSqlAdapter.Delete<CartDetail>(cartDetail.Id);
                }

                // Xóa giỏ hàng của khách hàng
                cartAdapter.Delete<Cart>(customerCart.Id);

                Console.WriteLine($"Customer cart cleared. Customer ID: {customerId}");
            }
            else
            {
                Console.WriteLine($"Customer cart not found for Customer ID: {customerId}");
            }
        }

        /// <summary>
        /// GetCustomerCart
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public Cart GetCustomerCart(Guid customerId)
        {
            // Get customer cart from the database
            List<Cart> customerCarts = cartAdapter.GetData<Cart>();
            return customerCarts.FirstOrDefault(c => c.CustomerId == customerId);
        }

        private List<CartDetail> GetCartDetailsByCartId(Guid Id)
        {
            // Get cart details based on cartId
            List <CartDetail> cartDetail=cartDetailSqlAdapter.GetData<CartDetail>().Where(cd => cd.Id == Id).ToList();
            return cartDetail;
        }

        /// <summary>
        /// GetCartDetail
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public CartDetail GetCartDetail(Guid Id, Guid productId)
        {
            var cartDetails = cartAdapter.GetData<CartDetail>();
            if (cartDetails != null && cartDetails.Any())
            {
                string UppperProductId = productId.ToString();
                var cartDetail = cartDetails?.FirstOrDefault(cd => cd?.Id == Id && cd?.ProductId == productId);


                if (cartDetail != null)
                {
                    var product = GetProductById(productId);
                    Console.WriteLine($"Found CartDetail: CartId = {cartDetail.Id}, ProductId = {cartDetail.ProductId},Name={product.Name}, Price = {product.Price} , Quantity = {cartDetail.Quantity}");

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
        public Product GetProductById(Guid productId)
        {
            
            var products = cartAdapter.GetData<Product>();
            return products.FirstOrDefault(p => p.ProductId == productId);
        }



    }
}
