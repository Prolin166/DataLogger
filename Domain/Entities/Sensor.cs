using Common;
using Domain.Entities;
using Domain.Enums;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Sensor
    {
        public Sensor(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public SensorType Type { get; set; }
        public ConnectionPort Port { get; set; }
        public EnableType Enabled { get; set; }
        public Unit Unit { get; set; }
        public virtual ICollection<Measurement> Measurements { get; set; }
    }
}