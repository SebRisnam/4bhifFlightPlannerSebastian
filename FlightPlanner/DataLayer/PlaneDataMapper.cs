using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
