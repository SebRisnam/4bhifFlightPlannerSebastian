using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.DataLayer
{
    internal class FlightDataMapper
    {
        public string ConnectionString { get; set; }

        public FlightDataMapper(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public List<Flight> ReadFlights()
        {
            List<Flight> flights = new List<Flight>();
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand selectFlightCommand = databaseConnection.CreateCommand();
                selectFlightCommand.CommandText = "SELECT * FROM Flight";
                databaseConnection.Open();

                IDataReader flightReader = selectFlightCommand.ExecuteReader();

                while (flightReader.Read())
                {
                    Flight flight = new Flight
                    {
                        Id = (int)flightReader["Id"], // Id is an int
                        Departure = (string)flightReader["Departure"],
                        Destination = (string)flightReader["Destination"],
                        Duration = (int)flightReader["Duration"],
                        DepartureDate = (DateTime)flightReader["DepartureDate"],
                        PlaneId = (int)flightReader["PlaneId"]
                    };

                    flights.Add(flight);
                }
            }
            return flights;
        }

        public Flight Read(int id)
        {
            Flight flight = null;
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand selectFlightCommand = databaseConnection.CreateCommand();
                selectFlightCommand.CommandText = "SELECT * FROM Flight WHERE Id = @Id";

                var idParameter = selectFlightCommand.CreateParameter();
                idParameter.ParameterName = "@Id";
                idParameter.Value = id;
                selectFlightCommand.Parameters.Add(idParameter);

                databaseConnection.Open();

                IDataReader flightReader = selectFlightCommand.ExecuteReader();

                if (flightReader.Read())
                {
                    flight = new Flight
                    {
                        Id = (int)flightReader["Id"], // Id is an int
                        Departure = (string)flightReader["Departure"],
                        Destination = (string)flightReader["Destination"],
                        Duration = (int)flightReader["Duration"],
                        DepartureDate = (DateTime)flightReader["DepartureDate"],
                        PlaneId = (int)flightReader["PlaneId"]
                    };
                }
            }
            return flight;
        }

        public int Create(Flight flight)
        {
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand createFlightCommand = databaseConnection.CreateCommand();

                createFlightCommand.CommandText =
                    "INSERT INTO Flight (Id, Departure, Destination, Duration, DepartureDate, PlaneId) " +
                    "VALUES (@Id, @Departure, @Destination, @Duration, @DepartureDate, @PlaneId);";

                var idParameter = createFlightCommand.CreateParameter();
                idParameter.ParameterName = "@Id";
                idParameter.Value = flight.Id;
                createFlightCommand.Parameters.Add(idParameter);

                var departureParameter = createFlightCommand.CreateParameter();
                departureParameter.ParameterName = "@Departure";
                departureParameter.Value = flight.Departure ?? (object)DBNull.Value;
                createFlightCommand.Parameters.Add(departureParameter);

                var destinationParameter = createFlightCommand.CreateParameter();
                destinationParameter.ParameterName = "@Destination";
                destinationParameter.Value = flight.Destination ?? (object)DBNull.Value;
                createFlightCommand.Parameters.Add(destinationParameter);

                var durationParameter = createFlightCommand.CreateParameter();
                durationParameter.ParameterName = "@Duration";
                durationParameter.Value = flight.Duration;
                createFlightCommand.Parameters.Add(durationParameter);

                var departureDateParameter = createFlightCommand.CreateParameter();
                departureDateParameter.ParameterName = "@DepartureDate";
                departureDateParameter.Value = flight.DepartureDate;
                createFlightCommand.Parameters.Add(departureDateParameter);

                var planeIdParameter = createFlightCommand.CreateParameter();
                planeIdParameter.ParameterName = "@PlaneId";
                planeIdParameter.Value = flight.PlaneId;
                createFlightCommand.Parameters.Add(planeIdParameter);

                Console.WriteLine(createFlightCommand.CommandText);
                databaseConnection.Open();

                int rowCount = createFlightCommand.ExecuteNonQuery();
                return rowCount;
            }
        }

        public int Update(Flight flight)
        {
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand updateFlightCommand = databaseConnection.CreateCommand();
                updateFlightCommand.CommandText =
                    "UPDATE Flight SET " +
                    "Departure = @Departure, " +
                    "Destination = @Destination, " +
                    "Duration = @Duration, " +
                    "DepartureDate = @DepartureDate, " +
                    "PlaneId = @PlaneId " +
                    "WHERE Id = @Id;";

                var idParameter = updateFlightCommand.CreateParameter();
                idParameter.ParameterName = "@Id";
                idParameter.Value = flight.Id;
                updateFlightCommand.Parameters.Add(idParameter);

                var departureParameter = updateFlightCommand.CreateParameter();
                departureParameter.ParameterName = "@Departure";
                departureParameter.Value = flight.Departure ?? (object)DBNull.Value;
                updateFlightCommand.Parameters.Add(departureParameter);

                var destinationParameter = updateFlightCommand.CreateParameter();
                destinationParameter.ParameterName = "@Destination";
                destinationParameter.Value = flight.Destination ?? (object)DBNull.Value;
                updateFlightCommand.Parameters.Add(destinationParameter);

                var durationParameter = updateFlightCommand.CreateParameter();
                durationParameter.ParameterName = "@Duration";
                durationParameter.Value = flight.Duration;
                updateFlightCommand.Parameters.Add(durationParameter);

                var departureDateParameter = updateFlightCommand.CreateParameter();
                departureDateParameter.ParameterName = "@DepartureDate";
                departureDateParameter.Value = flight.DepartureDate;
                updateFlightCommand.Parameters.Add(departureDateParameter);

                var planeIdParameter = updateFlightCommand.CreateParameter();
                planeIdParameter.ParameterName = "@PlaneId";
                planeIdParameter.Value = flight.PlaneId;
                updateFlightCommand.Parameters.Add(planeIdParameter);

                Console.WriteLine(updateFlightCommand.CommandText);
                databaseConnection.Open();

                int rowCount = updateFlightCommand.ExecuteNonQuery();
                return rowCount;
            }
        }

        public int Delete(Flight flight)
        {
            return Delete(flight.Id);
        }

        public int Delete(int id)
        {
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand deleteFlightCommand = databaseConnection.CreateCommand();
                deleteFlightCommand.CommandText = "DELETE FROM Flight WHERE Id = @Id;";

                var idParameter = deleteFlightCommand.CreateParameter();
                idParameter.ParameterName = "@Id";
                idParameter.Value = id;
                deleteFlightCommand.Parameters.Add(idParameter);

                Console.WriteLine(deleteFlightCommand.CommandText);
                databaseConnection.Open();

                int rowCount = deleteFlightCommand.ExecuteNonQuery();
                return rowCount;
            }
        }
    }
}