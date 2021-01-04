using System;
using System.Collections.Generic;
using System.Text;

namespace AutomaticTelephoneStation.DAL.Interfaces
{
    public interface ISubscriber : IEquatable<ISubscriber>
    {
        string FirstName { get; set; }
        string LastName { get; set; }

        string GetFullName();
    }
}
