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

        public int GetSeatsByFlightId(int flightId)
        {
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                // Create the command for fetching the seats based on FlightId
                IDbCommand seatCommand = databaseConnection.CreateCommand();

                // SQL query to get the number of seats for the plane related to the FlightId
                seatCommand.CommandText = @"
                    SELECT pt.Seats
                    FROM Flight f
                    INNER JOIN Plane p ON f.PlaneId = p.Id
                    INNER JOIN PlaneType pt ON p.PlaneTypeId = pt.Id
                    WHERE f.Id = @FlightId";

                var flightIdParameter = seatCommand.CreateParameter();
                flightIdParameter.ParameterName = "@FlightId";
                flightIdParameter.Value = flightId;
                seatCommand.Parameters.Add(flightIdParameter);

                databaseConnection.Open();

                object result = seatCommand.ExecuteScalar();

                if (result == DBNull.Value || result == null)
                {
                    return 0;
                }

                return Convert.ToInt32(result);
            }
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

                return Convert.ToInt32(result);
            }
        }

        #region Transaction
        public void CreateBookingTransaction(Booking booking, DbConnection databaseConnection)
        {
                try
                    {
                        IDbCommand createBookingCommand = databaseConnection.CreateCommand();

                        createBookingCommand.CommandText =
                            "INSERT INTO Booking (FlightId, CustomerId, Seats, TravelClass, Price) " +
                            "VALUES (@FlightId, @CustomerId, @Seats, @TravelClass, @Price);";

                        var flightIdParameter = createBookingCommand.CreateParameter();
                        flightIdParameter.ParameterName = "@FlightId";
                        flightIdParameter.Value = booking.FlightId;
                        createBookingCommand.Parameters.Add(flightIdParameter);

                        var customerIdParameter = createBookingCommand.CreateParameter();
                        customerIdParameter.ParameterName = "@CustomerId";
                        customerIdParameter.Value = booking.CustomerId;
                        createBookingCommand.Parameters.Add(customerIdParameter);

                        var seatsParameter = createBookingCommand.CreateParameter();
                        seatsParameter.ParameterName = "@Seats";
                        seatsParameter.Value = booking.Seats;
                        createBookingCommand.Parameters.Add(seatsParameter);

                        var travelClassParameter = createBookingCommand.CreateParameter();
                        travelClassParameter.ParameterName = "@TravelClass";
                        travelClassParameter.Value = booking.TravelClass;
                        createBookingCommand.Parameters.Add(travelClassParameter);

                        var priceParameter = createBookingCommand.CreateParameter();
                        priceParameter.ParameterName = "@Price";
                        priceParameter.Value = booking.Price;
                        createBookingCommand.Parameters.Add(priceParameter);

                        Console.WriteLine(createBookingCommand.CommandText);

                        int rowCount = createBookingCommand.ExecuteNonQuery();
                    }
                    catch (DbException dbEx)
                    {
                        throw new InvalidOperationException("Booking could not be created", dbEx);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
        
        }

        public int GetSeatsByFlightIdTransaction(int flightId, DbConnection databaseConnection)
        {
                // Create the command for fetching the seats based on FlightId
                IDbCommand seatCommand = databaseConnection.CreateCommand();

                // SQL query to get the number of seats for the plane related to the FlightId
                seatCommand.CommandText = @"
                    SELECT pt.Seats
                    FROM Flight f
                    INNER JOIN Plane p ON f.PlaneId = p.Id
                    INNER JOIN PlaneType pt ON p.PlaneTypeId = pt.Id
                    WHERE f.Id = @FlightId";

                var flightIdParameter = seatCommand.CreateParameter();
                flightIdParameter.ParameterName = "@FlightId";
                flightIdParameter.Value = flightId;
                seatCommand.Parameters.Add(flightIdParameter);

                object result = seatCommand.ExecuteScalar();

                if (result == DBNull.Value || result == null)
                {
                    return 0;
                }
                
                return Convert.ToInt32(result);
            
        }


        public int SumSeatsByFlightIdTransaction(int flightId, DbConnection databaseConnection)
        {
                IDbCommand sumSeatsCommand = databaseConnection.CreateCommand();
                sumSeatsCommand.CommandText = "SELECT SUM(Seats) AS TotalSeats FROM Booking WHERE FlightId = @FlightId;";

                var flightIdParameter = sumSeatsCommand.CreateParameter();
                flightIdParameter.ParameterName = "@FlightId";
                flightIdParameter.Value = flightId;
                sumSeatsCommand.Parameters.Add(flightIdParameter);

                object result = sumSeatsCommand.ExecuteScalar();

                if (result == DBNull.Value)
                {
                    return 0;
                }

                return Convert.ToInt32(result);
            
        }
        #endregion
    }
}
