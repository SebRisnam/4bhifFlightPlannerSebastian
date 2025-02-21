using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.DataLayer
{
    internal class BookingDataMapper
    {
        public string ConnectionString { get; set; }

        public BookingDataMapper(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public List<Booking> ReadBookings()
        {
            List<Booking> bookings = new List<Booking>();
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand selectBookingCommand = databaseConnection.CreateCommand();
                selectBookingCommand.CommandText = "SELECT * FROM Booking";
                databaseConnection.Open();

                IDataReader bookingReader = selectBookingCommand.ExecuteReader();

                while (bookingReader.Read())
                {
                    Booking booking = new Booking
                    {
                        FlightId = (int)bookingReader["FlightId"],
                        CustomerId = (int)bookingReader["CustomerId"],
                        Seats = (int)bookingReader["Seats"],
                        TravelClass = (int)bookingReader["TravelClass"],
                        Price = (decimal)bookingReader["Price"]
                    };

                    bookings.Add(booking);
                }
            }
            return bookings;
        }

        public Booking Read(int flightId, int customerId)
        {
            Booking booking = null;
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand selectBookingCommand = databaseConnection.CreateCommand();
                selectBookingCommand.CommandText = "SELECT * FROM Booking WHERE FlightId = @FlightId AND CustomerId = @CustomerId";

                var flightIdParameter = selectBookingCommand.CreateParameter();
                flightIdParameter.ParameterName = "@FlightId";
                flightIdParameter.Value = flightId;
                selectBookingCommand.Parameters.Add(flightIdParameter);

                var customerIdParameter = selectBookingCommand.CreateParameter();
                customerIdParameter.ParameterName = "@CustomerId";
                customerIdParameter.Value = customerId;
                selectBookingCommand.Parameters.Add(customerIdParameter);

                databaseConnection.Open();

                IDataReader bookingReader = selectBookingCommand.ExecuteReader();

                if (bookingReader.Read())
                {
                    booking = new Booking
                    {
                        FlightId = (int)bookingReader["FlightId"],
                        CustomerId = (int)bookingReader["CustomerId"],
                        Seats = (int)bookingReader["Seats"],
                        TravelClass = (int)bookingReader["TravelClass"],
                        Price = (decimal)bookingReader["Price"]
                    };
                }
            }
            return booking;
        }

        public int Create(Booking booking)
        {
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
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
                databaseConnection.Open();

                int rowCount = createBookingCommand.ExecuteNonQuery();
                return rowCount;
            }
        }

        public int Update(Booking booking)
        {
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand updateBookingCommand = databaseConnection.CreateCommand();
                updateBookingCommand.CommandText =
                    "UPDATE Booking SET " +
                    "Seats = @Seats, " +
                    "TravelClass = @TravelClass, " +
                    "Price = @Price " +
                    "WHERE FlightId = @FlightId AND CustomerId = @CustomerId;";

                var flightIdParameter = updateBookingCommand.CreateParameter();
                flightIdParameter.ParameterName = "@FlightId";
                flightIdParameter.Value = booking.FlightId;
                updateBookingCommand.Parameters.Add(flightIdParameter);

                var customerIdParameter = updateBookingCommand.CreateParameter();
                customerIdParameter.ParameterName = "@CustomerId";
                customerIdParameter.Value = booking.CustomerId;
                updateBookingCommand.Parameters.Add(customerIdParameter);

                var seatsParameter = updateBookingCommand.CreateParameter();
                seatsParameter.ParameterName = "@Seats";
                seatsParameter.Value = booking.Seats;
                updateBookingCommand.Parameters.Add(seatsParameter);

                var travelClassParameter = updateBookingCommand.CreateParameter();
                travelClassParameter.ParameterName = "@TravelClass";
                travelClassParameter.Value = booking.TravelClass;
                updateBookingCommand.Parameters.Add(travelClassParameter);

                var priceParameter = updateBookingCommand.CreateParameter();
                priceParameter.ParameterName = "@Price";
                priceParameter.Value = booking.Price;
                updateBookingCommand.Parameters.Add(priceParameter);

                Console.WriteLine(updateBookingCommand.CommandText);
                databaseConnection.Open();

                int rowCount = updateBookingCommand.ExecuteNonQuery();
                return rowCount;
            }
        }

        public int Delete(Booking booking)
        {
            return Delete(booking.FlightId, booking.CustomerId);
        }

        public int Delete(int flightId, int customerId)
        {
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand deleteBookingCommand = databaseConnection.CreateCommand();
                deleteBookingCommand.CommandText = "DELETE FROM Booking WHERE FlightId = @FlightId AND CustomerId = @CustomerId;";

                var flightIdParameter = deleteBookingCommand.CreateParameter();
                flightIdParameter.ParameterName = "@FlightId";
                flightIdParameter.Value = flightId;
                deleteBookingCommand.Parameters.Add(flightIdParameter);

                var customerIdParameter = deleteBookingCommand.CreateParameter();
                customerIdParameter.ParameterName = "@CustomerId";
                customerIdParameter.Value = customerId;
                deleteBookingCommand.Parameters.Add(customerIdParameter);

                Console.WriteLine(deleteBookingCommand.CommandText);
                databaseConnection.Open();

                int rowCount = deleteBookingCommand.ExecuteNonQuery();
                return rowCount;
            }
        }

        public int DeleteByFlightId(int flightId)
        {
            using (DbConnection databaseConnection = new SqlConnection(this.ConnectionString))
            {
                IDbCommand deleteBookingCommand = databaseConnection.CreateCommand();
                deleteBookingCommand.CommandText = "DELETE FROM Booking WHERE FlightId = @FlightId;";

                var flightIdParameter = deleteBookingCommand.CreateParameter();
                flightIdParameter.ParameterName = "@FlightId";
                flightIdParameter.Value = flightId;
                deleteBookingCommand.Parameters.Add(flightIdParameter);

                Console.WriteLine(deleteBookingCommand.CommandText);
                databaseConnection.Open();

                int rowCount = deleteBookingCommand.ExecuteNonQuery();
                return rowCount;
            }
        }
    }
}