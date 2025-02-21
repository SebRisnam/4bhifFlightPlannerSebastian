using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using FlightPlanner.DataLayer.Entities;

namespace FlightPlanner.DataLayer.DataMapper
{
    internal class PilotDataMapper
    {
        public string ConnectionString { get; set; }

        public PilotDataMapper(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public List<Pilot> ReadPilots()
        {
            List<Pilot> pilots = new List<Pilot>();
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand selectCommand = databaseConnection.CreateCommand();
                selectCommand.CommandText = "SELECT * FROM Pilot";
                databaseConnection.Open();

                IDataReader reader = selectCommand.ExecuteReader();
                while (reader.Read())
                {
                    Pilot pilot = new Pilot
                    {
                        Id = (int)reader["Id"],
                        LastName = (string)reader["LastName"],
                        Birthday = (DateTime)reader["Birthday"],
                        Qualification = (string)reader["Qualification"],
                        FlightHours = (int)reader["FlightHours"],
                        FirstDate = (DateTime)reader["FirstDate"],
                        AirlineId = (int)reader["AirlineId"]
                    };
                    pilots.Add(pilot);
                }
            }
            return pilots;
        }

        public Pilot Read(int id)
        {
            Pilot pilot = null;
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand selectCommand = databaseConnection.CreateCommand();
                selectCommand.CommandText = "SELECT * FROM Pilot WHERE Id = @Id";

                var idParameter = selectCommand.CreateParameter();
                idParameter.ParameterName = "@Id";
                idParameter.Value = id;
                selectCommand.Parameters.Add(idParameter);

                databaseConnection.Open();
                IDataReader reader = selectCommand.ExecuteReader();

                if (reader.Read())
                {
                    pilot = new Pilot
                    {
                        Id = (int)reader["Id"],
                        LastName = (string)reader["LastName"],
                        Birthday = (DateTime)reader["Birthday"],
                        Qualification = (string)reader["Qualification"],
                        FlightHours = (int)reader["FlightHours"],
                        FirstDate = (DateTime)reader["FirstDate"],
                        AirlineId = (int)reader["AirlineId"]
                    };
                }
            }
            return pilot;
        }

        public int Create(Pilot pilot)
        {
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand command = databaseConnection.CreateCommand();
                command.CommandText =
                    "INSERT INTO Pilot (LastName, Birthday, Qualification, FlightHours, FirstDate, AirlineId) " +
                    "VALUES (@LastName, @Birthday, @Qualification, @FlightHours, @FirstDate, @AirlineId);";

                var parameters = new[]
                {
                    new SqlParameter("@LastName", pilot.LastName),
                    new SqlParameter("@Birthday", pilot.Birthday),
                    new SqlParameter("@Qualification", pilot.Qualification),
                    new SqlParameter("@FlightHours", pilot.FlightHours),
                    new SqlParameter("@FirstDate", pilot.FirstDate),
                    new SqlParameter("@AirlineId", pilot.AirlineId)
                };

                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }

                databaseConnection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public int Update(Pilot pilot)
        {
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand command = databaseConnection.CreateCommand();
                command.CommandText =
                    "UPDATE Pilot SET " +
                    "LastName = @LastName, Birthday = @Birthday, Qualification = @Qualification, " +
                    "FlightHours = @FlightHours, FirstDate = @FirstDate, AirlineId = @AirlineId " +
                    "WHERE Id = @Id;";

                var parameters = new[]
                {
                    new SqlParameter("@Id", pilot.Id),
                    new SqlParameter("@LastName", pilot.LastName),
                    new SqlParameter("@Birthday", pilot.Birthday),
                    new SqlParameter("@Qualification", pilot.Qualification),
                    new SqlParameter("@FlightHours", pilot.FlightHours),
                    new SqlParameter("@FirstDate", pilot.FirstDate),
                    new SqlParameter("@AirlineId", pilot.AirlineId)
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
                command.CommandText = "DELETE FROM Pilot WHERE Id = @Id";

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
