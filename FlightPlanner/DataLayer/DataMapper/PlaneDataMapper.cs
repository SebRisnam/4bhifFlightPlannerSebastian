using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.DataLayer
{
    class PlaneDataMapper
    {
        public String ConnectionString { get; set; }

        public PlaneDataMapper(string ConnectionString)
        {
            this.ConnectionString = ConnectionString;
        }

        public List<Plane> ReadPlanes()
        {
            List<Plane> planes = new List<Plane>();
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand selectPlaneCommand = databaseConnection.CreateCommand();
                selectPlaneCommand.CommandText = "select * from Plane";
                databaseConnection.Open();

                IDataReader planeReader = selectPlaneCommand.ExecuteReader();
                while (planeReader.Read())
                {
                    // https://docs.microsoft.com/en-us/dotnet/api/system.data.idatarecord.item
                    Plane plane = new Plane();
                    plane.Id = (int)planeReader["Id"];
                    plane.OwnershipDate = (DateTime)planeReader["OwnershipDate"];
                    plane.LastMaintenance = (DateTime)planeReader["LastMaintenance"];
                    plane.PlaneTypeId = (string)planeReader["PlaneTypeId"];
                    plane.AirlineId = planeReader["AirlineId"] == DBNull.Value ? (int?)null : (int)planeReader["AirlineId"];


                    planes.Add(plane);
                }
            }
            return planes;
        }


        public Plane Read(int id)
        {
            Plane plane = null;

            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand selectPlaneCommand = databaseConnection.CreateCommand();
                selectPlaneCommand.CommandText = "SELECT * FROM Plane WHERE Id = @Id;";

                var idParameter = selectPlaneCommand.CreateParameter();
                idParameter.ParameterName = "@Id";
                idParameter.Value = id;
                selectPlaneCommand.Parameters.Add(idParameter);

                databaseConnection.Open();
                using (IDataReader planeReader = selectPlaneCommand.ExecuteReader())
                {
                    if (planeReader.Read())
                    {
                        plane = new Plane
                        {
                            Id = (int)planeReader["Id"],
                            OwnershipDate = (DateTime)planeReader["OwnershipDate"],
                            LastMaintenance = (DateTime)planeReader["LastMaintenance"],
                            PlaneTypeId = (string)planeReader["PlaneTypeId"],
                            AirlineId = planeReader["AirlineId"] == DBNull.Value ? (int?)null : (int)planeReader["AirlineId"]
                        };
                    }
                }
            }

            return plane;
        }


        public int Create(Plane plane)
        {
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                // 1. Create a command object for a raw SQL query
                IDbCommand createPlaneCommand = databaseConnection.CreateCommand();
                createPlaneCommand.CommandText =
                    "INSERT INTO Plane (Id, OwnershipDate, LastMaintenance, PlaneTypeId, AirlineId) " +
                    "VALUES (@Id, @OwnershipDate, @LastMaintenance, @PlaneTypeId, @AirlineId);";

                // 2. Add parameters to the command which will be passed to the SQL query
                IDbDataParameter param;

                param = createPlaneCommand.CreateParameter();
                param.ParameterName = "@Id";
                param.DbType = DbType.Int32;
                param.Value = plane.Id;
                createPlaneCommand.Parameters.Add(param);

                param = createPlaneCommand.CreateParameter();
                param.ParameterName = "@OwnershipDate";
                param.DbType = DbType.DateTime;
                param.Value = plane.OwnershipDate.ToString("s", System.Globalization.CultureInfo.InvariantCulture); // ISO 8601 format
                createPlaneCommand.Parameters.Add(param);

                param = createPlaneCommand.CreateParameter();
                param.ParameterName = "@LastMaintenance";
                param.DbType = DbType.DateTime;
                param.Value = plane.LastMaintenance.ToString("s", System.Globalization.CultureInfo.InvariantCulture); // ISO 8601 format
                createPlaneCommand.Parameters.Add(param);

                param = createPlaneCommand.CreateParameter();
                param.ParameterName = "@PlaneTypeId";
                param.DbType = DbType.String;
                param.Value = plane.PlaneTypeId;
                createPlaneCommand.Parameters.Add(param);

                param = createPlaneCommand.CreateParameter();
                param.ParameterName = "@AirlineId";
                param.DbType = DbType.Int32;
                param.Value = plane.AirlineId;
                createPlaneCommand.Parameters.Add(param);

                // 3. Open the database connection and execute the command
                databaseConnection.Open();
                int rowCount = createPlaneCommand.ExecuteNonQuery();

                return rowCount;
            }
        }



        public int Update(Plane plane)
        {
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand updatePlaneCommand = databaseConnection.CreateCommand();
                updatePlaneCommand.CommandText =
                    "UPDATE Plane SET " +
                    "OwnershipDate = @OwnershipDate, " +
                    "LastMaintenance = @LastMaintenance, " +
                    "PlaneTypeId = @PlaneTypeId, " +
                    "AirlineId = @AirlineId " +
                    "WHERE Id = @Id;";

                var idParameter = updatePlaneCommand.CreateParameter();
                idParameter.ParameterName = "@Id";
                idParameter.Value = plane.Id;
                updatePlaneCommand.Parameters.Add(idParameter);

                var ownershipDateParameter = updatePlaneCommand.CreateParameter();
                ownershipDateParameter.ParameterName = "@OwnershipDate";
                ownershipDateParameter.Value = plane.OwnershipDate;
                updatePlaneCommand.Parameters.Add(ownershipDateParameter);

                var lastMaintenanceParameter = updatePlaneCommand.CreateParameter();
                lastMaintenanceParameter.ParameterName = "@LastMaintenance";
                lastMaintenanceParameter.Value = plane.LastMaintenance;
                updatePlaneCommand.Parameters.Add(lastMaintenanceParameter);

                var planeTypeIdParameter = updatePlaneCommand.CreateParameter();
                planeTypeIdParameter.ParameterName = "@PlaneTypeId";
                planeTypeIdParameter.Value = plane.PlaneTypeId;
                updatePlaneCommand.Parameters.Add(planeTypeIdParameter);

                var airlineIdParameter = updatePlaneCommand.CreateParameter();
                airlineIdParameter.ParameterName = "@AirlineId";
                airlineIdParameter.Value = (object?)plane.AirlineId ?? DBNull.Value; // Handles null values correctly
                updatePlaneCommand.Parameters.Add(airlineIdParameter);

                Console.WriteLine(updatePlaneCommand.CommandText);
                databaseConnection.Open();

                int rowCount = updatePlaneCommand.ExecuteNonQuery();
                return rowCount;
            }
        }


        public int Delete(Plane plane)
        {
            return Delete(plane.Id);
        }

        public int Delete(int id)
        {
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand deletePlaneCommand = databaseConnection.CreateCommand();
                deletePlaneCommand.CommandText = "DELETE FROM Plane WHERE Id = @Id;";

                var idParameter = deletePlaneCommand.CreateParameter();
                idParameter.ParameterName = "@Id";
                idParameter.Value = id;
                deletePlaneCommand.Parameters.Add(idParameter);

                Console.WriteLine(deletePlaneCommand.CommandText);
                databaseConnection.Open();

                int rowCount = deletePlaneCommand.ExecuteNonQuery();
                return rowCount;
            }
        }


    }
}
