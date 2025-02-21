using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.DataLayer.Entities
{
    internal class Pilot
    {
        public Pilot() { }

        public Pilot(int id, string lastName, DateTime birthday, string qualification, int flightHours, DateTime firstDate, int airlineId)
        {
            Id = id;
            LastName = lastName;
            Birthday = birthday;
            Qualification = qualification;
            FlightHours = flightHours;
            FirstDate = firstDate;
            AirlineId = airlineId;
        }

        public int Id { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string Qualification { get; set; } // "Captain" or "Copilot"
        public int FlightHours { get; set; }
        public DateTime FirstDate { get; set; }
        public int AirlineId { get; set; } // Foreign key

        public override string ToString()
        {
            return $"Id: {Id}, LastName: {LastName}, Birthday: {Birthday:d}, " +
                   $"Qualification: {Qualification}, FlightHours: {FlightHours}, FirstDate: {FirstDate:d}, AirlineId: {AirlineId}";
        }
    }
}
