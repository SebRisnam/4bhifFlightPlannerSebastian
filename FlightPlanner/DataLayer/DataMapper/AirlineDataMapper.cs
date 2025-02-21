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
    class AirlineDataMapper
    {
        public String ConnectionString { get; set; }

        public AirlineDataMapper(string ConnectionString)
        {
            this.ConnectionString = ConnectionString;
        }

        public List<Airline> ReadAirlines()
        {
            List<Airline> airlines = new List<Airline>();
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand selectAirlineCommand = databaseConnection.CreateCommand();
                selectAirlineCommand.CommandText = "select * from Airline";
                databaseConnection.Open();

                IDataReader airlineReader = selectAirlineCommand.ExecuteReader();

                while (airlineReader.Read())
                {
                    // https://docs.microsoft.com/en-us/dotnet/api/system.data.idatarecord.item
                    Airline airline = new Airline();
                    airline.Id = (int)airlineReader["Id"];
                    airline.RegisteredCompanyName = (string)airlineReader["RegisteredCompanyName"];
                    airline.Country = (string)airlineReader["Country"];
                    airline.HeadQuarters = (string)airlineReader["Headquarters"];

                    airlines.Add(airline);
                }
            }
            return airlines;
        }

        public Airline Read(int Id)
        {
            Airline plane = new Airline();
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand selectAirlineCommand = databaseConnection.CreateCommand();
                selectAirlineCommand.CommandText = "select * from Airline where Id = " + Id;
                databaseConnection.Open();

                IDataReader airlineReader = selectAirlineCommand.ExecuteReader();

                Airline airline = new Airline();
                airline.Id = (int)airlineReader["Id"];
                airline.RegisteredCompanyName = (string)airlineReader["RegisteredCompanyName"];
                airline.Country = (string)airlineReader["Country"];
                airline.HeadQuarters = (string)airlineReader["HeadQuarters"];

            }
            return plane;
        }

        public int Create(Airline airline)
        {
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand createAirlineCommand = databaseConnection.CreateCommand();

                createAirlineCommand.CommandText =
                    "INSERT INTO Airline (Id, RegisteredCompanyName, Country, HeadQuarters) " +
                    "VALUES (@Id, @RegisteredCompanyName, @Country, @HeadQuarters);";

                var idParameter = createAirlineCommand.CreateParameter();
                idParameter.ParameterName = "@Id";
                idParameter.Value = airline.Id;
                createAirlineCommand.Parameters.Add(idParameter);

                var registeredCompanyNameParameter = createAirlineCommand.CreateParameter();
                registeredCompanyNameParameter.ParameterName = "@RegisteredCompanyName";
                registeredCompanyNameParameter.Value = airline.RegisteredCompanyName ?? (object)DBNull.Value;
                createAirlineCommand.Parameters.Add(registeredCompanyNameParameter);

                var countryParameter = createAirlineCommand.CreateParameter();
                countryParameter.ParameterName = "@Country";
                countryParameter.Value = airline.Country;
                createAirlineCommand.Parameters.Add(countryParameter);

                var headQuartersParameter = createAirlineCommand.CreateParameter();
                headQuartersParameter.ParameterName = "@HeadQuarters";
                headQuartersParameter.Value = airline.HeadQuarters;
                createAirlineCommand.Parameters.Add(headQuartersParameter);

                Console.WriteLine(createAirlineCommand.CommandText);
                databaseConnection.Open();

                int rowCount = createAirlineCommand.ExecuteNonQuery();
                return rowCount;
            }
        }


        public int Update(Airline airline)
        {
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand updateAirlineCommand = databaseConnection.CreateCommand();
                updateAirlineCommand.CommandText =
                    "UPDATE Airline SET " +
                    "RegisteredCompanyName = @RegisteredCompanyName, " +
                    "Country = @Country, " +
                    "HeadQuarters = @HeadQuarters " +
                    "WHERE Id = @Id;";

                var idParameter = updateAirlineCommand.CreateParameter();
                idParameter.ParameterName = "@Id";
                idParameter.Value = airline.Id;
                updateAirlineCommand.Parameters.Add(idParameter);

                var companyNameParameter = updateAirlineCommand.CreateParameter();
                companyNameParameter.ParameterName = "@RegisteredCompanyName";
                companyNameParameter.Value = airline.RegisteredCompanyName;
                updateAirlineCommand.Parameters.Add(companyNameParameter);

                var countryParameter = updateAirlineCommand.CreateParameter();
                countryParameter.ParameterName = "@Country";
                countryParameter.Value = airline.Country;
                updateAirlineCommand.Parameters.Add(countryParameter);

                var headquartersParameter = updateAirlineCommand.CreateParameter();
                headquartersParameter.ParameterName = "@HeadQuarters";
                headquartersParameter.Value = airline.HeadQuarters;
                updateAirlineCommand.Parameters.Add(headquartersParameter);

                Console.WriteLine(updateAirlineCommand.CommandText);
                databaseConnection.Open();

                int rowCount = updateAirlineCommand.ExecuteNonQuery();
                return rowCount;
            }
        }


        public int Delete(Airline airline)
        {
            return Delete(airline.Id);
        }

        public int Delete(int id)
        {
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand deleteAirlineCommand = databaseConnection.CreateCommand();
                deleteAirlineCommand.CommandText = "DELETE FROM Airline WHERE Id = @Id;";

                var idParameter = deleteAirlineCommand.CreateParameter();
                idParameter.ParameterName = "@Id";
                idParameter.Value = id;
                deleteAirlineCommand.Parameters.Add(idParameter);

                Console.WriteLine(deleteAirlineCommand.CommandText);
                databaseConnection.Open();

                int rowCount = deleteAirlineCommand.ExecuteNonQuery();
                return rowCount;
            }
        }

    }
}
