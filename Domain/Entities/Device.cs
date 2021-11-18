using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Device 
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public string IPAdress { get; set; }
        public string HostName { get; set; }
        public string MacAdress { get; set; }
        public int DaysToSave { get; set; }
    }
}
