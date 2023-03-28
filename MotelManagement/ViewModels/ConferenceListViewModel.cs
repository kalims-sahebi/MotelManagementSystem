using MotelManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.ViewModels
{
    public class ConferenceListViewModel
    {
        public IEnumerable<ConferenceRoomModel> ConferenceList { get; set; }
    }
}
