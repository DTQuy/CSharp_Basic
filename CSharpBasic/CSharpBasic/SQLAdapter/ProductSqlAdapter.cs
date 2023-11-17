using CSharpBasic.Object;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBasic.SQLAdapter
{
    public class ProductSqlAdapter : ISQLAdapter
    {
        public string ConnectionString { get; set; }
        public string TableName { get; set; }

        public ProductSqlAdapter(string connectionString)
        {
            this.ConnectionString = connectionString;
            this.TableName = "Product";
        }

        public int Delete<T>(Guid id) where T : class, new()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = $"DELETE FROM {TableName} WHERE product_id = @ProductId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductId", id);

                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting product: {ex.Message}");
                return 0;
            }
        }

        public T Get<T>(Guid id) where T : class, new()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = $"SELECT product_id, name_product, price FROM {TableName} WHERE product_id = @ProductId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductId", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Product product = new Product
                            {
                                ProductId = Guid.Parse(reader["product_id"].ToString()),
                                Name = reader["name_product"].ToString(),
                                Price = Convert.ToDecimal(reader["price"])
                            };

                            return product as T;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving product: {ex.Message}");
            }

            return null;
        }

        public List<T> GetData<T>() where T : class, new()
        {
            List<T> products = new List<T>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = $"SELECT product_id, name_product, price FROM {TableName}";
                    SqlCommand command = new SqlCommand(query, connection);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product product = new Product
                            {
                                ProductId = Guid.Parse(reader["product_id"].ToString()),
                                Name = reader["name_product"].ToString(),
                                Price = Convert.ToDecimal(reader["price"])
                            };

                            products.Add(product as T);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving products: {ex.Message}");
            }

            return products;
        }

        public int Insert<T>(T item) where T : class, new()
        {
            try
            {
                Product product = item as Product;

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = $"INSERT INTO {TableName} (product_id, name_product, price) VALUES (@ProductId, @Name, @Price)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductId", product.ProductId);
                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@Price", product.Price);

                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting product: {ex.Message}");
                return 0;
            }
        }

        public int Update<T>(T item) where T : class, new()
        {
            try
            {
                Product product = item as Product;

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = $"UPDATE {TableName} SET name_product = @Name, price = @Price WHERE product_id = @ProductId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@ProductId", product.ProductId);

                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating product: {ex.Message}");
                return 0;
            }
        }
    }
}