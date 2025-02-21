using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using FlightPlanner.DataLayer.Entities;

namespace FlightPlanner.DataLayer.DataMapper
{
    internal class PilotTrainingDataMapper
    {
        public string ConnectionString { get; set; }

        public PilotTrainingDataMapper(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public List<PilotTraining> ReadPilotTrainings()
        {
            List<PilotTraining> pilotTrainings = new List<PilotTraining>();
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand selectCommand = databaseConnection.CreateCommand();
                selectCommand.CommandText = "SELECT * FROM PilotTraining";
                databaseConnection.Open();

                IDataReader reader = selectCommand.ExecuteReader();
                while (reader.Read())
                {
                    PilotTraining pilotTraining = new PilotTraining
                    {
                        PilotId = (int)reader["PilotId"],
                        TrainingId = (int)reader["TrainingId"],
                        Date = (DateTime)reader["Date"]
                    };
                    pilotTrainings.Add(pilotTraining);
                }
            }
            return pilotTrainings;
        }

        public int Create(PilotTraining pilotTraining)
        {
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand command = databaseConnection.CreateCommand();
                command.CommandText =
                    "INSERT INTO PilotTraining (PilotId, TrainingId, Date) " +
                    "VALUES (@PilotId, @TrainingId, @Date);";

                var parameters = new[]
                {
                    new SqlParameter("@PilotId", pilotTraining.PilotId),
                    new SqlParameter("@TrainingId", pilotTraining.TrainingId),
                    new SqlParameter("@Date", pilotTraining.Date)
                };

                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }

                databaseConnection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public int Delete(int pilotId, int trainingId)
        {
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand command = databaseConnection.CreateCommand();
                command.CommandText = "DELETE FROM PilotTraining WHERE PilotId = @PilotId AND TrainingId = @TrainingId";

                var parameters = new[]
                {
                    new SqlParameter("@PilotId", pilotId),
                    new SqlParameter("@TrainingId", trainingId)
                };

                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }

                databaseConnection.Open();
                return command.ExecuteNonQuery();
            }
        }
    }
}
