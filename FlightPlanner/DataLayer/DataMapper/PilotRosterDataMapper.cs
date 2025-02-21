using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using FlightPlanner.DataLayer.Entities;

namespace FlightPlanner.DataLayer.DataMapper
{
    internal class PilotRosterDataMapper
    {
        public string ConnectionString { get; set; }

        public PilotRosterDataMapper(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public int Create(PilotRoster pilotRoster)
        {
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand command = databaseConnection.CreateCommand();
                command.CommandText =
                    "INSERT INTO PilotRoster (PilotId, FlightId) " +
                    "VALUES (@PilotId, @FlightId);";

                command.Parameters.Add(new SqlParameter("@PilotId", pilotRoster.PilotId));
                command.Parameters.Add(new SqlParameter("@FlightId", pilotRoster.FlightId));

                databaseConnection.Open();
                return command.ExecuteNonQuery();
            }
        }
    }
}
