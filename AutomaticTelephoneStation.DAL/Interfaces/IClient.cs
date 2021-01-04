using System;
using System.Collections.Generic;
using System.Text;

namespace AutomaticTelephoneStation.DAL.Interfaces
{
    public interface IClient : ISubscriber
    {
        IContract Contract { get; set; }
        ITerminal Terminal { get; set; }
    }
}
