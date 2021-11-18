using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class ControllerNotificationModel
    {
        public int SessionId { get; set; }

        public ICollection<MeasurementModel> Measurements;
    }
}
