using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using FlightPlanner.DataLayer.Entities;

namespace FlightPlanner.DataLayer.DataMapper
{
    internal class TrainingDataMapper
    {
        public string ConnectionString { get; set; }

        public TrainingDataMapper(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public List<Training> ReadTrainings()
        {
            List<Training> trainings = new List<Training>();
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand selectCommand = databaseConnection.CreateCommand();
                selectCommand.CommandText = "SELECT * FROM Training";
                databaseConnection.Open();

                IDataReader reader = selectCommand.ExecuteReader();
                while (reader.Read())
                {
                    Training training = new Training
                    {
                        Id = (int)reader["Id"],
                        Description = (string)reader["Description"],
                        Level = (int)reader["Level"]
                    };
                    trainings.Add(training);
                }
            }
            return trainings;
        }
    }
}
