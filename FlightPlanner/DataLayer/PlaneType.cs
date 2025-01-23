using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.DataLayer
{
    internal class PlaneType
    {
        public PlaneType() { }

        public PlaneType(string Id, int Seats, int Velocity) { 
            this.Id = Id;
            this.Seats = Seats;
            this.Velocity = Velocity;
        }

        public string Id { get; set; }
        public int Seats { get; set; }
        public int Velocity { get; set; }
    }
}
