using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace FlightPlanner.DataLayer
{
    // Implement operations that affect several tables (e.g. deleting a flight affects also the Booking table)
    class FlightRepository
    {
        private BookingDataMapper bookingDataMapper;
        private FlightDataMapper flightDataMapper;
        private PlaneDataMapper planeDataMapper;
        private PlaneTypeDataMapper planeTypeDataMapper;
        // TODO: add other data mappers
        string ConnectionString { get; set; }

        public FlightRepository(string connectionString)
        {
            this.ConnectionString = connectionString;
            bookingDataMapper = new BookingDataMapper(this.ConnectionString);
            flightDataMapper = new FlightDataMapper(this.ConnectionString);
            planeDataMapper = new PlaneDataMapper(this.ConnectionString);
            planeTypeDataMapper = new PlaneTypeDataMapper(this.ConnectionString);
        }

        public void DeleteFlight(int id)
        {
            int rowCount = Int32.MinValue;
            try
            {
                // FK_Booking_Flight uses "on delete no action"
                // FK_PilotRoster_Flight uses "ON DELETE CASCADE"
                rowCount = bookingDataMapper.DeleteByFlightId(id);
                rowCount = flightDataMapper.Delete(id);
            }
            catch (DbException dbEx) // TODO: review and improve exception handling
            {
                // TODO: log to log file
                throw new InvalidOperationException("Flight could not be deleted!", dbEx);
            }
            catch (Exception)
            {
                // TODO: log to log file
                throw;
            }

            if (rowCount < 1)
            {
                throw new InvalidOperationException("The specified flight could not be deleted.");
            }
        }

        public void DeleteFlightsByPlaneId(int planeId) 
        {
            List<Flight> flights = flightDataMapper.ReadFlights();
            foreach (Flight flight in flights)
            {
                DeleteFlight(flight.Id);
            }
        }

        public void CreateBooking(Booking booking)
        {
            bookingDataMapper.Create(booking);
        }

        public int GetSeatsOfPlaneByFlightId(int FlightId)
        {
            int planeId = flightDataMapper.Read(FlightId).PlaneId;
            string planeType = planeDataMapper.Read(planeId).PlaneTypeId;
            int seats = planeTypeDataMapper.Read(planeType).Seats;
            return seats;
        }

        public int SumSeatsByFlightId(int flightId)
        {
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand sumSeatsCommand = databaseConnection.CreateCommand();
                sumSeatsCommand.CommandText = "SELECT SUM(Seats) AS TotalSeats FROM Booking WHERE FlightId = @FlightId;";

                var flightIdParameter = sumSeatsCommand.CreateParameter();
                flightIdParameter.ParameterName = "@FlightId";
                flightIdParameter.Value = flightId;
                sumSeatsCommand.Parameters.Add(flightIdParameter);

                databaseConnection.Open();

                object result = sumSeatsCommand.ExecuteScalar();

                // If no bookings exist for the FlightId, result will be DBNull.Value
                if (result == DBNull.Value)
                {
                    return 0; // No seats booked for this FlightId
                }

                return Convert.ToInt32(result); // Return the total number of seats
            }
        }

    }
}
