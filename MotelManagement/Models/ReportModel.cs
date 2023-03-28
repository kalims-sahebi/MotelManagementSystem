using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.Models
{
    public class ReportModel
    {
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string DateString { get; set; }
        public int MoneyIn { get; set; }
        public int MoneyOut { get; set; }
    }
}
