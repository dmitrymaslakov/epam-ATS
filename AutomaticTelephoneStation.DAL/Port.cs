using AutomaticTelephoneStation.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomaticTelephoneStation.DAL
{
    public class Port : IPort
    {
        public PortState State { get; set; } = PortState.Disconnected;
    }
}
