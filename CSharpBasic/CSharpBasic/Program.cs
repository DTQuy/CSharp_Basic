using System;
using System.Collections.Generic;
using CSharpBasic.SQLAdapter;
using CSharpBasic.Object;
using CSharpBasic.Object;
using CSharpBasic.SQLAdapter;

namespace CSharpBasic
{
    class Program
    {
        static void Main()
        {
            string connectionString = "Server=TQ23\\SQLEXPRESS;Database=csharp_basic;Integrated Security=True;";

            bool exit = false;
            do
            {
                Console.WriteLine("Select table:");
                Console.WriteLine("1. Customers");
                Console.WriteLine("2. Products");
                Console.WriteLine("0. Exit");

                Console.Write("Enter your choice: ");
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        HandleCustomerTable(connectionString);
                        break;
                    case 2:
                        HandleProductTable(connectionString);
                        break;
                    case 0:
                        Console.WriteLine("Exiting in 5 seconds...");
                        System.Threading.Thread.Sleep(5000);
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            } while (!exit);
        }

        static void HandleCustomerTable(string connectionString)
        {
            CustomerSqlAdapter customerAdapter = new CustomerSqlAdapter(connectionString);

            Console.WriteLine("Select operation:");
            Console.WriteLine("1. View Customers");
            Console.WriteLine("2. Add Customer");
            Console.WriteLine("3. Update Customer");
            Console.WriteLine("4. Delete Customer");
            Console.WriteLine("0. Back");

            Console.Write("Enter your choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    ViewCustomers(customerAdapter);
                    break;
                case 2:
                    AddCustomer(customerAdapter);
                    break;
                case 3:
                    UpdateCustomer(customerAdapter);
                    break;
                case 4:
                    DeleteCustomer(customerAdapter);
                    break;
                case 0:
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        static void ViewCustomers(CustomerSqlAdapter customerAdapter)
        {
            List<Customer> customers = customerAdapter.GetData<Customer>();

            if (customers.Count > 0)
            {
                Console.WriteLine("Customers:");
                foreach (var customer in customers)
                {
                    Console.WriteLine($"Customer ID: {customer.Id}, Name: {customer.FirstName} {customer.LastName}, Email: {customer.Email}, Phone: {customer.PhoneNumber}, Address: {customer.Address}");
                }
            }
            else
            {
                Console.WriteLine("No customers found.");
            }
        }

        static void AddCustomer(CustomerSqlAdapter customerAdapter)
        {
            Console.Write("Enter first name: ");
            string firstName = Console.ReadLine();

            Console.Write("Enter last name: ");
            string lastName = Console.ReadLine();

            Console.Write("Enter email: ");
            string email = Console.ReadLine();

            Console.Write("Enter phone number: ");
            string phoneNumber = Console.ReadLine();

            Console.Write("Enter address: ");
            string address = Console.ReadLine();

            Customer newCustomer = new Customer
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phoneNumber,
                Address = address
            };

            int result = customerAdapter.Insert(newCustomer);

            if (result > 0)
            {
                Console.WriteLine("Customer added successfully.");
            }
            else
            {
                Console.WriteLine("Error adding customer.");
            }
        }

        static void UpdateCustomer(CustomerSqlAdapter customerAdapter)
        {
            Console.Write("Enter customer ID to update: ");
            Guid customerId = Guid.Parse(Console.ReadLine());

            Customer existingCustomer = customerAdapter.Get<Customer>(customerId);

            if (existingCustomer != null)
            {
                Console.Write("Enter new first name (press Enter to keep current): ");
                string newFirstName = Console.ReadLine();
                if (!string.IsNullOrEmpty(newFirstName))
                {
                    existingCustomer.FirstName = newFirstName;
                }

                Console.Write("Enter new last name (press Enter to keep current): ");
                string newLastName = Console.ReadLine();
                if (!string.IsNullOrEmpty(newLastName))
                {
                    existingCustomer.LastName = newLastName;
                }

                Console.Write("Enter new email (press Enter to keep current): ");
                string newEmail = Console.ReadLine();
                if (!string.IsNullOrEmpty(newEmail))
                {
                    existingCustomer.Email = newEmail;
                }

                Console.Write("Enter new phone number (press Enter to keep current): ");
                string newPhoneNumber = Console.ReadLine();
                if (!string.IsNullOrEmpty(newPhoneNumber))
                {
                    existingCustomer.PhoneNumber = newPhoneNumber;
                }

                Console.Write("Enter new address (press Enter to keep current): ");
                string newAddress = Console.ReadLine();
                if (!string.IsNullOrEmpty(newAddress))
                {
                    existingCustomer.Address = newAddress;
                }

                int result = customerAdapter.Update(existingCustomer);

                if (result > 0)
                {
                    Console.WriteLine("Customer updated successfully.");
                }
                else
                {
                    Console.WriteLine("Error updating customer.");
                }
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
        }

        static void DeleteCustomer(CustomerSqlAdapter customerAdapter)
        {
            Console.Write("Enter customer ID to delete: ");
            Guid customerId = Guid.Parse(Console.ReadLine());

            int result = customerAdapter.Delete<Customer>(customerId);

            if (result > 0)
            {
                Console.WriteLine("Customer deleted successfully.");
            }
            else
            {
                Console.WriteLine("Error deleting customer.");
            }
        }

        static void HandleProductTable(string connectionString)
        {
            ISQLAdapter productAdapter = new ProductSqlAdapter(connectionString);

            Console.WriteLine("Product Table Operations:");
            Console.WriteLine("1. View Products");
            Console.WriteLine("2. Add Product");
            Console.WriteLine("3. Update Product");
            Console.WriteLine("4. Delete Product");

            Console.Write("Enter your choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    ViewProducts(productAdapter);
                    break;
                case 2:
                    AddProduct(productAdapter);
                    break;
                case 3:
                    UpdateProduct(productAdapter);
                    break;
                case 4:
                    DeleteProduct(productAdapter);
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        static void ViewProducts(ISQLAdapter productAdapter)
        {
            List<Product> products = productAdapter.GetData<Product>();
            Console.WriteLine("Products:");
            foreach (var product in products)
            {
                Console.WriteLine($"ID: {product.Id}, Name: {product.Name}, Price: {product.Price}");
            }
        }

        static void AddProduct(ISQLAdapter productAdapter)
        {
            Console.Write("Enter product name: ");
            string name = Console.ReadLine();

            Console.Write("Enter product price: ");
            decimal price = Convert.ToDecimal(Console.ReadLine());

            Product newProduct = new Product
            {
                Id = Guid.NewGuid(),
                Name = name,
                Price = price
            };

            int result = productAdapter.Insert(newProduct);

            if (result > 0)
                Console.WriteLine("Product added successfully.");
            else
                Console.WriteLine("Failed to add product.");
        }

        static void UpdateProduct(ISQLAdapter productAdapter)
        {
            Console.Write("Enter product ID to update: ");
            Guid productId = Guid.Parse(Console.ReadLine());

            Product existingProduct = productAdapter.Get<Product>(productId);

            if (existingProduct != null)
            {
                Console.Write("Enter new product name: ");
                existingProduct.Name = Console.ReadLine();

                Console.Write("Enter new product price: ");
                existingProduct.Price = Convert.ToDecimal(Console.ReadLine());

                int result = productAdapter.Update(existingProduct);

                if (result > 0)
                    Console.WriteLine("Product updated successfully.");
                else
                    Console.WriteLine("Failed to update product.");
            }
            else
            {
                Console.WriteLine("Product not found.");
            }
        }

        static void DeleteProduct(ISQLAdapter productAdapter)
        {
            Console.Write("Enter product ID to delete: ");
            Guid productId = Guid.Parse(Console.ReadLine());

            int result = productAdapter.Delete<Product>(productId);

            if (result > 0)
                Console.WriteLine("Product deleted successfully.");
            else
                Console.WriteLine("Failed to delete product.");
        }
    }
}