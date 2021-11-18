using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class OperationTimer
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public DateTime LastTime { get; set; }
        public TimeSpan CumulatedCount { get; set; }

    }
}
