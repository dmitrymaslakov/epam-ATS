using System;
using System.Collections.Generic;
using System.Text;

namespace AutomaticTelephoneStation.DAL.Interfaces
{
    public interface IPort
    {
        PortState State { get; set; }
    }
}
