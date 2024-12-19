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
                   $"insert into Airline values ({airline.Id}, '{airline.RegisteredCompanyName}', " +
                   $"'{airline.Country}', '{airline.HeadQuarters}');";
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
                   $"update Airline set " +
                   $"RegisteredCompanyName = '{airline.RegisteredCompanyName}', " +
                   $"Country = '{airline.Country}', " +
                   $"HeadQuarters = '{airline.HeadQuarters}' " +
                   $"where Id = {airline.Id};";

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
        
        public int Delete(int Id)
        {
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand deletePlaneCommand = databaseConnection.CreateCommand();
                deletePlaneCommand.CommandText =
                   $"delete from Airline where Airline.Id = {Id};";

                Console.WriteLine(deletePlaneCommand.CommandText);

                databaseConnection.Open();

                int rowCount = deletePlaneCommand.ExecuteNonQuery();
                return rowCount;
            }

        }
    }
}
