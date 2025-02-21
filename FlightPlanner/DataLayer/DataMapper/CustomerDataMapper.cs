using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using FlightPlanner.DataLayer.Entities;

namespace FlightPlanner.DataLayer.DataMapper
{
    internal class CustomerDataMapper
    {
        public string ConnectionString { get; set; }

        public CustomerDataMapper(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public List<Customer> ReadCustomers()
        {
            List<Customer> customers = new List<Customer>();
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand selectCommand = databaseConnection.CreateCommand();
                selectCommand.CommandText = "SELECT * FROM Customer";
                databaseConnection.Open();

                IDataReader reader = selectCommand.ExecuteReader();
                while (reader.Read())
                {
                    Customer customer = new Customer
                    {
                        Id = (int)reader["Id"],
                        LastName = (string)reader["LastName"],
                        Birthday = (DateTime)reader["Birthday"],
                        City = (string)reader["City"]
                    };
                    customers.Add(customer);
                }
            }
            return customers;
        }

        public Customer Read(int id)
        {
            Customer customer = null;
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand selectCommand = databaseConnection.CreateCommand();
                selectCommand.CommandText = "SELECT * FROM Customer WHERE Id = @Id";

                var idParameter = selectCommand.CreateParameter();
                idParameter.ParameterName = "@Id";
                idParameter.Value = id;
                selectCommand.Parameters.Add(idParameter);

                databaseConnection.Open();
                IDataReader reader = selectCommand.ExecuteReader();

                if (reader.Read())
                {
                    customer = new Customer
                    {
                        Id = (int)reader["Id"],
                        LastName = (string)reader["LastName"],
                        Birthday = (DateTime)reader["Birthday"],
                        City = (string)reader["City"]
                    };
                }
            }
            return customer;
        }

        public int Create(Customer customer)
        {
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand command = databaseConnection.CreateCommand();
                command.CommandText =
                    "INSERT INTO Customer (LastName, Birthday, City) " +
                    "VALUES (@LastName, @Birthday, @City);";

                var parameters = new[]
                {
                    new SqlParameter("@LastName", customer.LastName),
                    new SqlParameter("@Birthday", customer.Birthday),
                    new SqlParameter("@City", customer.City)
                };

                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }

                databaseConnection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public int Update(Customer customer)
        {
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand command = databaseConnection.CreateCommand();
                command.CommandText =
                    "UPDATE Customer SET LastName = @LastName, Birthday = @Birthday, City = @City " +
                    "WHERE Id = @Id;";

                var parameters = new[]
                {
                    new SqlParameter("@Id", customer.Id),
                    new SqlParameter("@LastName", customer.LastName),
                    new SqlParameter("@Birthday", customer.Birthday),
                    new SqlParameter("@City", customer.City)
                };

                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }

                databaseConnection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public int Delete(int id)
        {
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand command = databaseConnection.CreateCommand();
                command.CommandText = "DELETE FROM Customer WHERE Id = @Id";

                var idParameter = command.CreateParameter();
                idParameter.ParameterName = "@Id";
                idParameter.Value = id;
                command.Parameters.Add(idParameter);

                databaseConnection.Open();
                return command.ExecuteNonQuery();
            }
        }
    }
}
