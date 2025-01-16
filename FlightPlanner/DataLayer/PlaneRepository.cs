using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.DataLayer
{
    internal class PlaneRepository
    {
        private FlightRepository flightRepository;
        private PlaneDataMapper planeDataMapper;
        // TODO: add other data mappers
        string ConnectionString { get; set; }

        public PlaneRepository(string connectionString)
        {
            this.ConnectionString = connectionString;
            flightRepository = new FlightRepository(connectionString);
            planeDataMapper = new PlaneDataMapper(connectionString);
        }

        public void DeletePlane(int id)
        {
            int rowCount = Int32.MinValue;
            try
            {
                // FK_Booking_Flight uses "on delete no action"
                // FK_PilotRoster_Flight uses "ON DELETE CASCADE"
                rowCount = flightRepository.DeleteFlightByPlaneId(id);
                rowCount = planeDataMapper.Delete(id);
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

            if (rowCount < 1)
            {
                throw new InvalidOperationException("The specified Airline could not be deleted");
            }
        }
    }
}
