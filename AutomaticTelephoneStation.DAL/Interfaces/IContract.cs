using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace AutomaticTelephoneStation.DAL.Interfaces
{
    public interface IContract : IEquatable<IContract>
    {
        IStation Operator { get; }
        IClient Client { get; }
        DateTime ContractDay { get; }
        ITerminal Terminal { get; }

        bool TrySign(IStation _operator, IClient client);
    }
}
