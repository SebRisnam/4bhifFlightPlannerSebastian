using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.DataLayer.Entities
{
    internal class Customer
    {
        public Customer() { }

        public Customer(int id, string lastName, DateTime birthday, string city)
        {
            Id = id;
            LastName = lastName;
            Birthday = birthday;
            City = city;
        }

        public int Id { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string City { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, LastName: {LastName}, Birthday: {Birthday:d}, City: {City}";
        }
    }
}
