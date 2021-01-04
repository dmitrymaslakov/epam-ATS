using AutomaticTelephoneStation.DAL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AutomaticTelephoneStation.DAL.Interfaces
{
    public interface IStation
    {
        public Dictionary<string, IPort> Ports { get; }

        public IEnumerable<IContract> Contracts { get; }

        public Dictionary<ITerminal, CallLog> CallsHistory { get; }

        bool TryAddContract(IContract contract);

        void TryConnectTo(string calledNumber, string caller);

        bool CheckExistedClient(IClient client);

        void CollectCallInformation(object sender, IContact contact);

        ITerminal FindBy(Expression<Func<ITerminal, bool>> searchPredicate);
    }
}