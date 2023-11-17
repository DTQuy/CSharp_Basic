using CSharpBasic.Object;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBasic.SQLAdapter
{
    public class CartDetailSqlAdapter: ISQLAdapter
    {
        public string ConnectionString { get; set; }
        public string TableName { get; set; }

        public CartDetailSqlAdapter(string connectionString)
        {
            this.ConnectionString = connectionString;
            this.TableName = "cart_details";
        }
        public int Insert<T>(T item) where T : class, new()
        {
            try
            {
                CartDetail cartDetail = item as CartDetail;
                

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = $"INSERT INTO dbo.{TableName} (cart_id, product_id, quantity) VALUES (@Id, @ProductId, @Quantity)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", cartDetail.Id);
                    command.Parameters.AddWithValue("@ProductId", cartDetail.ProductId);
                    command.Parameters.AddWithValue("@Quantity", cartDetail.Quantity);

                    return command.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting cart: {ex.Message}");
                return 0;
            }
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
                        CartDetail cartDetail = new CartDetail
                        {
                            Id = Guid.Parse(reader["cart_id"].ToString()),
                            ProductId = Guid.Parse(reader["product_id"].ToString()),
                            Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                        };

                        result.Add(cartDetail as T);
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
                        CartDetail cartDetail = new CartDetail
                        {
                            Id = Guid.Parse(reader["cart_id"].ToString()),
                            ProductId = Guid.Parse(reader["customer_id"].ToString()),
                            Quantity = reader.GetInt32(reader.GetOrdinal("quantity"))
                        };

                        return cartDetail as T;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting cart: {ex.Message}");
            }

            return null;
        }

        public int Update<T>(T item) where T : class, new()
        {
            try
            {
                CartDetail cartDetail = item as CartDetail;

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = $"UPDATE {TableName} SET product_id = @ProductId , Quantity = quantity WHERE cart_id = @Id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", cartDetail.Id);
                    command.Parameters.AddWithValue("@ProductId", cartDetail.ProductId);
                    command.Parameters.AddWithValue("@Quantity", cartDetail.Quantity);

                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating cart: {ex.Message}");
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

                    string query = $"DELETE FROM {TableName} WHERE cart_id = @Id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", id);

                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting cart detail: {ex.Message}");
                return 0;
            }
        }
    }
}
