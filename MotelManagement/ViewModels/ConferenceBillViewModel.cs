using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.ViewModels
{
    public class ConferenceBillViewModel
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public Double Hourse { get; set; }
        public Double total { get; set; }
        public int Duration { get; set; }
        public int RentPerHour { get; set; }
        public int RefreshmentCost { get; set; }
    }
}
