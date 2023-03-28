using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.ViewModels
{
    public class SafeBalanceViewModel
    {

        public int SafeBalancingId { get; set; }
        public int MoneyIn { get; set; }
        public int MoneyOut { get; set; }
        [Required(ErrorMessage ="تاریخ الزامی است")]
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string user { get; set; }
        public int SafeId { get; set; }
        public int Total { get; set; }
        public int CustomerId { get; set; }
        public int ConferenceRoomId { get; set; }


        public IEnumerable<SafeBalanceViewModel> SafeBalanceList { get; set; }
    }
}
