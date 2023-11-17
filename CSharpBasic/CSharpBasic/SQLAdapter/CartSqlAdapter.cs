using CSharpBasic.Object;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CSharpBasic.SQLAdapter
{
    public class CartSqlAdapter : ISQLAdapter
    {
        public string ConnectionString { get; set; }
        public string TableName { get; set; }

        public CartSqlAdapter(string connectionString)
        {
            this.ConnectionString = connectionString;
            this.TableName = "Cart";
        }

        public int Delete<T>(Guid id) where T : class, new()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = $"DELETE FROM {TableName} WHERE cart_id = @Id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", id);

                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting cart: {ex.Message}");
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

                    string query = $"SELECT * FROM {TableName} WHERE cart_id = @Id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", id);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        Cart cart = new Cart
                        {
                            Id = Guid.Parse(reader["cart_id"].ToString()),
                            CustomerId = Guid.Parse(reader["customer_id"].ToString())
                        };

                        return cart as T;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting cart: {ex.Message}");
            }

            return null;
        }

        public List<T> GetData<T>() where T : class, new()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = $"SELECT * FROM {TableName}";
                    SqlCommand command = new SqlCommand(query, connection);

                    SqlDataReader reader = command.ExecuteReader();

                    List<T> result = new List<T>();

                    while (reader.Read())
                    {
                        Cart cart = new Cart
                        {
                            Id = Guid.Parse(reader["cart_id"].ToString()),
                            CustomerId = Guid.Parse(reader["customer_id"].ToString())
                        };

                        result.Add(cart as T);
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting cart data: {ex.Message}");
                return null;
            }
        }

        public int Insert<T>(T item) where T : class, new()
        {
            try
            {
                Cart cart = item as Cart;

                if (cart != null)
                {
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        connection.Open();

                        string query = $"INSERT INTO {TableName} (cart_id, customer_id) VALUES (@Id, @CustomerId)";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Id", cart.Id);
                        command.Parameters.AddWithValue("@CustomerId", cart.CustomerId);

                        return command.ExecuteNonQuery();
                    }
                }
                else
                {
                    Console.WriteLine("Error: Cart is null.");
                    return 0; 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting cart: {ex.Message}");
                return 0;
            }
        }
       

        public int Update<T>(T item) where T : class, new()
        {
            try
            {
                Cart cart = item as Cart;

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = $"UPDATE {TableName} SET customer_id = @CustomerId WHERE cart_id = @Id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", cart.Id);
                    command.Parameters.AddWithValue("@CustomerId", cart.CustomerId);

                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating cart: {ex.Message}");
                return 0;
            }
        }

        int ISQLAdapter.Insert<T>(T item)
        {
            throw new NotImplementedException();
        }
    }
}
