using System;
using System.Collections.Generic;
using System.Text;

namespace AutomaticTelephoneStation.DAL
{
    public class ActiveCall
    {
        public Guid Id { get; set; }
        public string CallerNumber { get; set; }
        public string CalledNumber { get; set; }
    }
}
