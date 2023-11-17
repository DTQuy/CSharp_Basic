using CSharpBasic.Object;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CSharpBasic.SQLAdapter
{
    public class OrderDetailSqlAdapter : ISQLAdapter
    {
        public string ConnectionString { get; set; }
        public string TableName { get; set; }

        public OrderDetailSqlAdapter(string connectionString)
        {
            this.ConnectionString = connectionString;
            this.TableName = "order_details";
        }

        public int Insert<T>(T item) where T : class, new()
        {
            try
            {
                Order order = item as Order;

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = $"INSERT INTO {TableName} (id_order, customer_id, order_day, total_amount) VALUES (@Id, @CustomerId, @OrderDay, @TotalAmount)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", order.Id);
                    command.Parameters.AddWithValue("@CustomerId", order.CustomerId);
                    command.Parameters.AddWithValue("@OrderDay", order.OrderDay);
                    command.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);

                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting order: {ex.Message}");
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

                    string query = $"SELECT * FROM {TableName} WHERE id_order = @Id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", id);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        Order order = new Order
                        {
                            Id = Guid.Parse(reader["id_order"].ToString()),
                            CustomerId = Guid.Parse(reader["customer_id"].ToString()),
                            OrderDay = DateTime.Parse(reader["order_day"].ToString()),
                            TotalAmount = decimal.Parse(reader["total_amount"].ToString())
                        };

                        return order as T;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting order: {ex.Message}");
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

                    List<T> orders = new List<T>();

                    while (reader.Read())
                    {
                        Order order = new Order
                        {
                            Id = Guid.Parse(reader["id_order"].ToString()),
                            CustomerId = Guid.Parse(reader["customer_id"].ToString()),
                            OrderDay = DateTime.Parse(reader["order_day"].ToString()),
                            TotalAmount = decimal.Parse(reader["total_amount"].ToString())
                        };

                        orders.Add(order as T);
                    }

                    return orders;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting orders: {ex.Message}");
            }

            return null;
        }

        public int Update<T>(T item) where T : class, new()
        {
            try
            {
                Order order = item as Order;

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = $"UPDATE {TableName} SET customer_id = @CustomerId, order_day = @OrderDay, total_amount = @TotalAmount WHERE id_order = @Id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", order.Id);
                    command.Parameters.AddWithValue("@CustomerId", order.CustomerId);
                    command.Parameters.AddWithValue("@OrderDay", order.OrderDay);
                    command.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);

                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating order: {ex.Message}");
                return 0;
            }
        }

        public int Delete<T>(Guid id) where T : class, new()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = $"DELETE FROM {TableName} WHERE id_order = @Id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", id);

                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting order: {ex.Message}");
                return 0;
            }
        }
    }
}
