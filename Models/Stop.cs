using System;
using System.Collections.Generic;

namespace TripPlanner.Models
{
    public class Stop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Order { get; set; }
        public DateTime ArrivalTime { get; set; }

        public Trip Trip { get; set; }
    }
}