using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Measurement
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public double Value { get; set; }
        public int SensorId { get; set; }
        public Sensor Sensor { get; set; }


    }
}