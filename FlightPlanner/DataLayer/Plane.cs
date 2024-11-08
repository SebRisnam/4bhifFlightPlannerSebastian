using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.DataLayer
{
    class Plane
    {
        public int Id { get; set; }
        public DateTime OwnershipDate { get; set; }
        public DateTime LastMaintenance {  get; set; }
        public string PlaneTypeId { get; set; }
        public int AirlineId { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, OnwershipDate: {OwnershipDate}, LastMaintenance: {LastMaintenance}, " +
                $"PlaneTypeId: {PlaneTypeId}, AirlineId: {AirlineId}";
        }
    }
}
