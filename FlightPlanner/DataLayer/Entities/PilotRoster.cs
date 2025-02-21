using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.DataLayer.Entities
{
    internal class PilotRoster
    {
        public PilotRoster() { }

        public PilotRoster(int pilotId, int flightId)
        {
            PilotId = pilotId;
            FlightId = flightId;
        }

        public int PilotId { get; set; } // Foreign key
        public int FlightId { get; set; } // Foreign key

        public override string ToString()
        {
            return $"PilotId: {PilotId}, FlightId: {FlightId}";
        }
    }
}
