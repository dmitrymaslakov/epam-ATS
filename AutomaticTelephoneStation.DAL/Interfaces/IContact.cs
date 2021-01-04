using System;
using System.Collections.Generic;
using System.Text;

namespace AutomaticTelephoneStation.DAL.Interfaces
{
    public interface IContact : ISubscriber
    {
        string Number { get; set; }
    }
}
