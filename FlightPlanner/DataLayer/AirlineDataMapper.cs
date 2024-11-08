using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.DataLayer
{
    class AirlineDataMapper
    {
        public String ConnectionString { get; set; }

        public AirlineDataMapper(string ConnectionString)
        {
            this.ConnectionString = ConnectionString;
        }
    }
}
