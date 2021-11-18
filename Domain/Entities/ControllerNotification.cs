using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ControllerNotification
    {
        public int SessionId { get; set; }

        public ICollection<Measurement> Measurements;
    }
}
