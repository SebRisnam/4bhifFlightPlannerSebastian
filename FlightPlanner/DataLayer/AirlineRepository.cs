using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.DataLayer
{
    internal class AirlineRepository
    {
        private AirlineDataMapper airlineDataMapper;
        private PlaneRepository planeRepository;
        private PlaneDataMapper planeDataMapper;
        // TODO: add other data mappers
        string ConnectionString { get; set; }

        public AirlineRepository(string connectionString)
        {
            this.ConnectionString = connectionString;
            airlineDataMapper = new AirlineDataMapper(connectionString);
            planeRepository = new PlaneRepository(connectionString);
            planeDataMapper = new PlaneDataMapper(connectionString);
        }

        public void DeleteAirline(int id)
        {
            int rowCount = Int32.MinValue;
            try
            {
                // FK_Booking_Flight uses "on delete no action"
                // FK_PilotRoster_Flight uses "ON DELETE CASCADE"
                DeletePlanesByAirlineID(id);
                rowCount = airlineDataMapper.Delete(id);
            }
            catch (DbException dbEx) // TODO: review and improve exception handling
            {
                // TODO: log to log file
                throw new InvalidOperationException("Airline could not be deleted", dbEx);
            }
            catch (Exception)
            {
                // TODO: log to log file
                throw;
            }
        }
        public int DeletePlanesByAirlineID(int airlineID)
        {
            int rowCount = Int32.MinValue;
            List<Plane> planes = planeDataMapper.ReadPlanes();
            foreach (Plane item in planes) {
                if (item.AirlineId == airlineID)
                {
                    planeRepository.DeletePlane(item.Id);
                }
            }
            return rowCount;
        }
    }
}
