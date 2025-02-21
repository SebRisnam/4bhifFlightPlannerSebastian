using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.DataLayer.Entities
{
    internal class PilotTraining
    {
        public PilotTraining() { }

        public PilotTraining(int pilotId, int trainingId, DateTime date)
        {
            PilotId = pilotId;
            TrainingId = trainingId;
            Date = date;
        }

        public int PilotId { get; set; } // Foreign key
        public int TrainingId { get; set; } // Foreign key
        public DateTime Date { get; set; }

        public override string ToString()
        {
            return $"PilotId: {PilotId}, TrainingId: {TrainingId}, Date: {Date:d}";
        }
    }
}
