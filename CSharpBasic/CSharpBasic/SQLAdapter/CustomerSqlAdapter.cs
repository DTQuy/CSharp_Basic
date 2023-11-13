using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CSharpBasic.Object;
using CSharpBasic.Object;
using CSharpBasic.SQLAdapter;

namespace CSharpBasic.SQLAdapter
{
    public class CustomerSqlAdapter : ISQLAdapter
    {
        public string ConnectionString { get; set; }

        public string TableName { get; set; }

        public CustomerSqlAdapter(string connectionString)
        {
            this.ConnectionString = connectionString;
            this.TableName = "Customer";
        }

        public int Delete<T>(Guid id) where T : class, new()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = $"DELETE FROM {TableName} WHERE customer_id = @Id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", id);

                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting Customer: {ex.Message}");
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

                    string query = $"SELECT customer_id, first_name, last_name, email, phone_number, address FROM {TableName} WHERE customer_id = @Id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Customer customer = new Customer
                            {
                                Id = Guid.Parse(reader["customer_id"].ToString()),
                                FirstName = reader["first_name"].ToString(),
                                LastName = reader["last_name"].ToString(),
                                Email = reader["email"].ToString(),
                                PhoneNumber = reader["phone_number"].ToString(),
                                Address = reader["address"].ToString()
                            };

                            return customer as T;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving customer: {ex.Message}");
            }

            return null;
        }

        public List<T> GetData<T>() where T : class, new()
        {
            List<T> customers = new List<T>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = $"SELECT customer_id, first_name, last_name, email, phone_number, address FROM {TableName}";
                    SqlCommand command = new SqlCommand(query, connection);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Customer customer = new Customer
                            {
                                Id = Guid.Parse(reader["customer_id"].ToString()),
                                FirstName = reader["first_name"].ToString(),
                                LastName = reader["last_name"].ToString(),
                                Email = reader["email"].ToString(),
                                PhoneNumber = reader["phone_number"].ToString(),
                                Address = reader["address"].ToString()
                            };

                            customers.Add(customer as T);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving customers: {ex.Message}");
            }

            return customers;
        }

        public int Insert<T>(T item) where T : class, new()
        {
            try
            {
                Customer customer = item as Customer;

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = $"INSERT INTO {TableName} (customer_id, first_name, last_name, email, phone_number, address) VALUES (@Id, @FirstName, @LastName, @Email, @PhoneNumber, @Address)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", customer.Id);
                    command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    command.Parameters.AddWithValue("@LastName", customer.LastName);
                    command.Parameters.AddWithValue("@Email", customer.Email);
                    command.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
                    command.Parameters.AddWithValue("@Address", customer.Address);

                    return command.ExecuteNonQuery() ;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting customer: {ex.Message}");
                return default;
            }
        }

        public int Update<T>(T item) where T : class, new()
        {
            try
            {
                Customer customer = item as Customer;

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = $"UPDATE {TableName} SET first_name = @FirstName, last_name = @LastName, email = @Email, phone_number = @PhoneNumber, address = @Address WHERE customer_id = @Id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    command.Parameters.AddWithValue("@LastName", customer.LastName);
                    command.Parameters.AddWithValue("@Email", customer.Email);
                    command.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
                    command.Parameters.AddWithValue("@Address", customer.Address);
                    command.Parameters.AddWithValue("@Id", customer.Id);

                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating customer: {ex.Message}");
                return 0;
            }
        }

        int ISQLAdapter.Insert<T>(T item)
        {
            throw new NotImplementedException();
        }
    }
}
