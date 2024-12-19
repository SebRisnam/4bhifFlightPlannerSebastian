using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.DataLayer
{
    class Airline
    {
        public int Id { get; set; }
        public string RegisteredCompanyName { get; set; }
        public string Country { get; set; }
        public string HeadQuarters { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, RegisteredCompanyName: {RegisteredCompanyName}, " + 
                $"Country: {Country}, Headquarters: {HeadQuarters}";
        }


    }
}
