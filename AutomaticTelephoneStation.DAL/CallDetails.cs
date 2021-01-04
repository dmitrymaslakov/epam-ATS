using AutomaticTelephoneStation.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomaticTelephoneStation.DAL
{
    public class CallDetails
    {
        public CallTypes CallType { get; set; }
        public IContact Contact { get; set; }
        public DateTime Beginning { get; set; }
        public TimeSpan Duration { get; set; }
        public decimal Cost { get; set; }
    }
}
