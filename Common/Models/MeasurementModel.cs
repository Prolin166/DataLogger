using System;
using System.Collections.Generic;

namespace Common.Models
{
    public class MeasurementModel
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public double Value { get; set; }
        public int SensorId { get; set; }
        public SensorModel Sensor { get; set; }


    }
}