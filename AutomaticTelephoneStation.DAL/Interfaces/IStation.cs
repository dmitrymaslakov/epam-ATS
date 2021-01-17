using AutomaticTelephoneStation.DAL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AutomaticTelephoneStation.DAL.Interfaces
{
    public interface IStation
    {
        event EventHandler<(string calledNumber, string callerNumber)> ConnectionRequest;

        event EventHandler<(string calledNumber, string callerNumber)> FinishRequest;

        public Dictionary<string, IPort> Ports { get; }

        public IEnumerable<IContract> Contracts { get; }

        public Dictionary<ITerminal, CallLog> CallsHistory { get; }

        IEnumerable<ActiveCall> ActiveCalls { get; }

        bool TryAddContract(IContract contract);

        void TryConnectTo(string calledNumber, string caller);

        void TerminateCall(string calledNumber, string callerNumber);

        bool CheckExistedClient(IClient client);

        void CollectCallInformation(string calledNumber, string callerNumber);

        void ReturnCallParticipantsToInitialState(string calledNumber, string caller);

        ITerminal FindBy(Expression<Func<ITerminal, bool>> searchPredicate);

        void AddActiveCall(string calledNumber, string caller);

        void RemoveActiveCall(ActiveCall activeCall);

        IEnumerable<CallDetails> GetCallsIndex(ITerminal Terminal, Expression<Func<CallDetails, bool>> searchPredicate = null);

        void SendVoiceMessageTo(string number, string message);
    }
}