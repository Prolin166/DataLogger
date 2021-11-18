using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class OperationTimerModel
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public DateTime LastTime { get; set; }
        public TimeSpan CumulatedCount { get; set; }

    }
}
