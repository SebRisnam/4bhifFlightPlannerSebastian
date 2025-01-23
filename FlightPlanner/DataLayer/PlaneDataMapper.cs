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


        public Plane Read(int Id)
        {
            Plane plane = new Plane();
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand selectPlaneCommand = databaseConnection.CreateCommand();
                selectPlaneCommand.CommandText = "select * from Plane where Id = " + Id;
                databaseConnection.Open();
                IDataReader planeReader = selectPlaneCommand.ExecuteReader();

                plane.Id = (int)planeReader["Id"];
                plane.OwnershipDate = (DateTime)planeReader["OwnershipDate"];
                plane.LastMaintenance = (DateTime)planeReader["LastMaintenance"];
                plane.PlaneTypeId = (string)planeReader["PlaneTypeId"];
                plane.AirlineId = planeReader["AirlineId"] == DBNull.Value ? (int?)null : (int)planeReader["AirlineId"];


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
                   $"update Plane set OwnershipDate = '{plane.OwnershipDate.ToString("s", System.Globalization.CultureInfo.InvariantCulture)}', " +
                   $"LastMaintenance = '{plane.LastMaintenance.ToString("s", System.Globalization.CultureInfo.InvariantCulture)}', " +
                   $"PlaneTypeId = '{plane.PlaneTypeId}', " +
                   $"AirLineId = '{plane.AirlineId}' " +
                   $"where Plane.Id = {plane.Id};";
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

        public int Delete(int Id)
        {
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand deletePlaneCommand = databaseConnection.CreateCommand();
                deletePlaneCommand.CommandText =
                   $"delete from Plane where Plane.Id = {Id};";

                Console.WriteLine(deletePlaneCommand.CommandText);

                databaseConnection.Open();

                int rowCount = deletePlaneCommand.ExecuteNonQuery();
                return rowCount;
            }

        }

    }
}
