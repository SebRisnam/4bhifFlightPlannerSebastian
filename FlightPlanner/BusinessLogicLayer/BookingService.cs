using FlightPlanner.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.BusinessLogicLayer
{
    class BookingService
    {
        private FlightRepository flightRepository;
        private string connectionString;

        public BookingService(string connectionString)
        {
            this.connectionString = connectionString;
            flightRepository = new FlightRepository(connectionString);
        }

        public void BookFlight(int FlightId, int CustomerId, int Seats, int TravelClass, decimal Price)
        {
            //get the Flight --> get Plane --> get PlaneType --> get seats
            //get the taken seats --> Logic to refuse or not
            //make Booking and save it in the db

            //get the Seats of the plane
            int availableSeats = flightRepository.GetSeatsByFlightId(FlightId);
            //get taken Seats
            int takenSeats = flightRepository.SumSeatsByFlightId(FlightId);
            
            if (Seats <= availableSeats - takenSeats)
            {
                //make Booking
                Booking booking = new Booking(FlightId, CustomerId, Seats, TravelClass, Price);
                flightRepository.CreateBooking(booking);
                Console.WriteLine("Booking has successfully been made!");
            }
            else
            {
                Console.WriteLine("I'm sorry, this flight has no available Seats");
            }
        }
    }
}
