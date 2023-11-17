using CSharpBasic.Object;
using CSharpBasic.SQLAdapter;
using System;
using System.Collections.Generic;

namespace CSharpBasic.BussinessService
{
    public class OrderService
    {
        private readonly OrderSqlAdapter orderAdapter;
        private readonly OrderDetailSqlAdapter orderDetailSqlAdapter;
        private readonly CartService cartService;
        private readonly CartSqlAdapter cartSqlAdapter;
        private readonly CartDetailSqlAdapter cartDetailSqlAdapter;

        public OrderService(string connectionString)
        {
            this.orderAdapter = new OrderSqlAdapter(connectionString);
            this.orderDetailSqlAdapter = new OrderDetailSqlAdapter(connectionString);
            this.cartService = new CartService(connectionString);
            this.cartSqlAdapter = new CartSqlAdapter(connectionString); 
            this.cartDetailSqlAdapter = new CartDetailSqlAdapter(connectionString);
        }

        public void ClearCustomerCart(Guid customerId)
        {
            Cart customerCart = GetCustomerCart(customerId);

            if (customerCart != null)
            {
                List<CartDetail> cartDetails = GetCartDetailsByCartId(customerCart.Id);
                foreach (var cartDetail in cartDetails)
                {
                    cartDetailSqlAdapter.Delete<CartDetail>(cartDetail.Id);
                }

                cartSqlAdapter.Delete<Cart>(customerCart.Id);

                Console.WriteLine($"Customer cart cleared. Customer ID: {customerId}");
            }
            else
            {
                Console.WriteLine($"Customer cart not found for Customer ID: {customerId}");
            }
        }
        public void CreateOrder(Guid customerId)
        {
            // Retrieve the customer's cart details
            List<CartDetail> cartDetails = cartService.GetCartDetails(customerId);

            if (cartDetails != null && cartDetails.Any())
            {
                // Calculate the total amount
                decimal totalAmount = 0;
                foreach (var cartDetail in cartDetails)
                {
                    Product product = cartService.GetProductById(cartDetail.ProductId);
                    if (product != null)
                    {
                        totalAmount += cartDetail.Quantity * product.Price;
                    }
                }

                // Create an order
                Order newOrder = new Order
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customerId,
                    OrderDay = DateTime.Now, // Set the order date to the current date
                    TotalAmount = totalAmount
                };

                // Insert the order into the database
                int result = orderAdapter.Insert(newOrder);

                if (result > 0)
                {
                    Console.WriteLine("Order created successfully.");

                    // Clear the customer's cart after creating the order
                    cartService.ClearCustomerCart(customerId);
                }
                else
                {
                    Console.WriteLine("Error creating order.");
                }
            }
            else
            {
                Console.WriteLine("No items in the customer's cart. Unable to create an order.");
            }
        }


        public void ViewOrders()
        {
            // Retrieve and display all orders
            List<Order> orders = orderAdapter.GetData<Order>();

            if (orders != null && orders.Any())
            {
                Console.WriteLine("Orders:");

                foreach (var order in orders)
                {
                    Console.WriteLine($"Order ID: {order.Id}, Customer ID: {order.CustomerId}, Order Date: {order.OrderDay}, Total Amount: {order.TotalAmount}");
                }
            }
            else
            {
                Console.WriteLine("No orders found.");
            }
        }

        public void CalculateTotalAmount(List<CartDetail> cartDetails)
        {
            // Calculate total amount based on product prices and quantities
            decimal totalAmount = 0;

            foreach (var cartDetail in cartDetails)
            {
                Product product = cartService.GetProductById(cartDetail.ProductId);
                if (product != null)
                {
                    totalAmount += cartDetail.Quantity * product.Price;
                }
            }

            Console.WriteLine($"Total amount: {totalAmount}");
        }

        private Cart GetCustomerCart(Guid customerId)
        {
            // Get customer cart from the database
            List<Cart> customerCarts = cartSqlAdapter.GetData<Cart>();
            return customerCarts.FirstOrDefault(c => c.CustomerId == customerId);
        }

        private List<CartDetail> GetCartDetailsByCartId(Guid Id)
        {
            // Get cart details based on cartId
            List<CartDetail> cartDetail = cartDetailSqlAdapter.GetData<CartDetail>().Where(cd => cd.Id == Id).ToList();
            return cartDetail;
        }
    }
}

