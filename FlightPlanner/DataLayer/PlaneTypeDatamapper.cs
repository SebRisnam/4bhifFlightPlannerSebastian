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
    internal class PlaneTypeDataMapper
    {
        public string ConnectionString { get; set; }

        public PlaneTypeDataMapper(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public List<PlaneType> ReadPlaneTypes()
        {
            List<PlaneType> planeTypes = new List<PlaneType>();
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand selectPlaneTypeCommand = databaseConnection.CreateCommand();
                selectPlaneTypeCommand.CommandText = "SELECT * FROM PlaneType";
                databaseConnection.Open();

                IDataReader planeTypeReader = selectPlaneTypeCommand.ExecuteReader();

                while (planeTypeReader.Read())
                {
                    PlaneType planeType = new PlaneType
                    {
                        Id = (string)planeTypeReader["Id"], // Id is now a string
                        Seats = (int)planeTypeReader["Seats"],
                        Velocity = (int)planeTypeReader["Velocity"]
                    };

                    planeTypes.Add(planeType);
                }
            }
            return planeTypes;
        }

        public PlaneType Read(string id)
        {
            PlaneType planeType = null;
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand selectPlaneTypeCommand = databaseConnection.CreateCommand();
                selectPlaneTypeCommand.CommandText = "SELECT * FROM PlaneType WHERE Id = @Id";

                var idParameter = selectPlaneTypeCommand.CreateParameter();
                idParameter.ParameterName = "@Id";
                idParameter.Value = id;
                selectPlaneTypeCommand.Parameters.Add(idParameter);

                databaseConnection.Open();

                IDataReader planeTypeReader = selectPlaneTypeCommand.ExecuteReader();

                if (planeTypeReader.Read())
                {
                    planeType = new PlaneType
                    {
                        Id = (string)planeTypeReader["Id"], // Id is now a string
                        Seats = (int)planeTypeReader["Seats"],
                        Velocity = (int)planeTypeReader["Velocity"]
                    };
                }
            }
            return planeType;
        }

        public int Create(PlaneType planeType)
        {
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand createPlaneTypeCommand = databaseConnection.CreateCommand();

                createPlaneTypeCommand.CommandText =
                    "INSERT INTO PlaneType (Id, Seats, Velocity) " +
                    "VALUES (@Id, @Seats, @Velocity);";

                var idParameter = createPlaneTypeCommand.CreateParameter();
                idParameter.ParameterName = "@Id";
                idParameter.Value = planeType.Id; // Id is now a string
                createPlaneTypeCommand.Parameters.Add(idParameter);

                var seatsParameter = createPlaneTypeCommand.CreateParameter();
                seatsParameter.ParameterName = "@Seats";
                seatsParameter.Value = planeType.Seats;
                createPlaneTypeCommand.Parameters.Add(seatsParameter);

                var velocityParameter = createPlaneTypeCommand.CreateParameter();
                velocityParameter.ParameterName = "@Velocity";
                velocityParameter.Value = planeType.Velocity;
                createPlaneTypeCommand.Parameters.Add(velocityParameter);

                Console.WriteLine(createPlaneTypeCommand.CommandText);
                databaseConnection.Open();

                int rowCount = createPlaneTypeCommand.ExecuteNonQuery();
                return rowCount;
            }
        }

        public int Update(PlaneType planeType)
        {
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand updatePlaneTypeCommand = databaseConnection.CreateCommand();
                updatePlaneTypeCommand.CommandText =
                    "UPDATE PlaneType SET " +
                    "Seats = @Seats, " +
                    "Velocity = @Velocity " +
                    "WHERE Id = @Id;";

                var idParameter = updatePlaneTypeCommand.CreateParameter();
                idParameter.ParameterName = "@Id";
                idParameter.Value = planeType.Id; // Id is now a string
                updatePlaneTypeCommand.Parameters.Add(idParameter);

                var seatsParameter = updatePlaneTypeCommand.CreateParameter();
                seatsParameter.ParameterName = "@Seats";
                seatsParameter.Value = planeType.Seats;
                updatePlaneTypeCommand.Parameters.Add(seatsParameter);

                var velocityParameter = updatePlaneTypeCommand.CreateParameter();
                velocityParameter.ParameterName = "@Velocity";
                velocityParameter.Value = planeType.Velocity;
                updatePlaneTypeCommand.Parameters.Add(velocityParameter);

                Console.WriteLine(updatePlaneTypeCommand.CommandText);
                databaseConnection.Open();

                int rowCount = updatePlaneTypeCommand.ExecuteNonQuery();
                return rowCount;
            }
        }

        public int Delete(PlaneType planeType)
        {
            return Delete(planeType.Id);
        }

        public int Delete(string id)
        {
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand deletePlaneTypeCommand = databaseConnection.CreateCommand();
                deletePlaneTypeCommand.CommandText = "DELETE FROM PlaneType WHERE Id = @Id;";

                var idParameter = deletePlaneTypeCommand.CreateParameter();
                idParameter.ParameterName = "@Id";
                idParameter.Value = id; // Id is now a string
                deletePlaneTypeCommand.Parameters.Add(idParameter);

                Console.WriteLine(deletePlaneTypeCommand.CommandText);
                databaseConnection.Open();

                int rowCount = deletePlaneTypeCommand.ExecuteNonQuery();
                return rowCount;
            }
        }
    }
}