using System;
using System.Collections.Generic;
using System.Text;

namespace AutomaticTelephoneStation.DAL
{
    public class CallLog
    {
        public ICollection<CallDetails> Calls { get; set; }
    }
}
