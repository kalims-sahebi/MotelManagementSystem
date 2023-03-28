using MotelManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.ViewModels
{
    public class ReportViewModel
    {
        public IEnumerable<ReportModel> Report { get; set; }
    }
}
