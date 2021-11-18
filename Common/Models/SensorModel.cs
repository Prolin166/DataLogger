using Common.Enums;
using System.Collections.Generic;

namespace Common.Models
{
    public class SensorModel
    {
        public SensorModel()
        {
        }
        public SensorModel(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public SensorTypeModel Type { get; set; }
        public ConnectionPortModel Port { get; set; }
        public EnableTypeModel Enabled { get; set; }
        public UnitModel Unit { get; set; }
        public virtual ICollection<MeasurementModel> Measurements { get; set; }

    }
}