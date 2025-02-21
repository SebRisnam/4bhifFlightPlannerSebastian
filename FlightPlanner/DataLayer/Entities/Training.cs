using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.DataLayer.Entities
{
    internal class Training
    {
        public Training() { }

        public Training(int id, string description, int level)
        {
            Id = id;
            Description = description;
            Level = level;
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public int Level { get; set; } // Must be between 1 and 5

        public override string ToString()
        {
            return $"Id: {Id}, Description: {Description}, Level: {Level}";
        }
    }
}
