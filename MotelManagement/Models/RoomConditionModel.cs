using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.Models
{
    public class RoomConditionModel
    {
        //used to see if room is full to generat the bill
        //in print bill by RoomNumber Action method
        public int IsEmpty { get; set; }
    }
}
