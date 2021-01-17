using AutomaticTelephoneStation.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AutomaticTelephoneStation.DAL
{
    public partial class TelecomOperator : IStation
    {
        public event EventHandler<(string calledNumber, string callerNumber)> ConnectionRequest;
        public event EventHandler<(string calledNumber, string callerNumber)> FinishRequest;
        private ICollection<IContract> _contracts;
        private ICollection<ActiveCall> _activeCalls;

        public Dictionary<string, IPort> Ports { get; set; }
        public IEnumerable<IContract> Contracts => _contracts;
        public Dictionary<ITerminal, CallLog> CallsHistory { get; set; }
        public IEnumerable<ActiveCall> ActiveCalls => _activeCalls;

        public TelecomOperator()
        {
            _contracts = new List<IContract>();
            _activeCalls = new List<ActiveCall>();
            Ports = new Dictionary<string, IPort>();
            CallsHistory = new Dictionary<ITerminal, CallLog>();
        }

        protected virtual void OnConnectionRequest(object sender, string calledNumber, string caller)
        {
            ConnectionRequest?.Invoke(sender, (calledNumber, caller));
        }

        protected virtual void OnFinishRequest(object sender, string calledNumber, string caller)
        {
            FinishRequest?.Invoke(sender, (calledNumber, caller));
        }

        public bool TryAddContract(IContract contract)
        {
            var existContract = _contracts
                .SingleOrDefault(c => c.Equals(contract));

            if (existContract != null)
            {
                return false;
            }
            else
            {
                _contracts.Add(contract);
                Ports.Add(contract.Terminal.Number, new Port());
                return true;
            }
        }

        public bool CheckExistedClient(IClient client)
        {
            var existContract = Contracts
                ?.SingleOrDefault(c => c.Client.Equals(client));

            return existContract == null;
        }

        public void CollectCallInformation(string calledNumber, string callerNumber)
        {
            var callerTerminal = FindBy(t => t.Number.Equals(callerNumber));
            var calledTerminal = FindBy(t => t.Number.Equals(calledNumber));
            var duration = TurnOnCounter();
            var cost = Count(duration);

            var callDetailsForCaller = new CallDetails
            {
                Beginning = RandomDay(),
                CallType = CallTypes.Outgoing,
                Contact = callerTerminal.FindBy(c => c.Number.Equals(calledTerminal.Number)),
                Duration = duration,
                Cost = cost.outgoing
            };

            var callDetailsForCalledParty = new CallDetails
            {
                Beginning = callDetailsForCaller.Beginning,
                CallType = CallTypes.Incoming,
                Contact = calledTerminal.FindBy(c => c.Number.Equals(callerTerminal.Number)),
                Duration = duration,
                Cost = cost.incoming
            };

            if (CallsHistory.Keys.Contains(callerTerminal))
            {
                CallsHistory[callerTerminal].Calls.Add(callDetailsForCaller);
            }
            else
            {
                CallsHistory.Add(
                    callerTerminal, new CallLog { Calls = new List<CallDetails> { callDetailsForCaller } });
            }

            if (CallsHistory.Keys.Contains(calledTerminal))
            {
                CallsHistory[calledTerminal].Calls.Add(callDetailsForCalledParty);
            }
            else
            {
                CallsHistory.Add(
                    calledTerminal, new CallLog { Calls = new List<CallDetails> { callDetailsForCalledParty } });
            }
        }

        public void ReturnCallParticipantsToInitialState(string calledNumber, string callerNumber)
        {
            Ports[calledNumber].State = PortState.Free;
            Ports[callerNumber].State = PortState.Free;
        }

        public void TryConnectTo(string calledNumber, string caller)
        {
            OnConnectionRequest(this, calledNumber, caller);
        }

        public void TerminateCall(string calledNumber, string caller)
        {
            OnFinishRequest(this, calledNumber, caller);
        }

        public ITerminal FindBy(Expression<Func<ITerminal, bool>> searchPredicate)
        {
            try
            {

                return Contracts.Select(c => c.Terminal).AsQueryable().SingleOrDefault(searchPredicate);
            }
            catch
            {
                throw;
            }

        }

        public void AddActiveCall(string calledNumber, string callerNumber)
        {
            var activeCall = new ActiveCall
            {
                Id = Guid.NewGuid(),
                CalledNumber = calledNumber,
                CallerNumber = callerNumber
            };
            FindBy(t => t.Number.Equals(callerNumber)).ActiveCall = activeCall;
            FindBy(t => t.Number.Equals(calledNumber)).ActiveCall = activeCall;
            _activeCalls.Add(activeCall);
        }

        public void RemoveActiveCall(ActiveCall activeCall)
        {
            FindBy(t => t.Number.Equals(activeCall.CallerNumber)).ActiveCall = null;
            FindBy(t => t.Number.Equals(activeCall.CalledNumber)).ActiveCall = null;
            _activeCalls.Remove(activeCall);
        }

        public IEnumerable<CallDetails> GetCallsIndex(ITerminal Terminal, Expression<Func<CallDetails, bool>> searchPredicate = null)
        {
            if (CallsHistory.Keys.Contains(Terminal))
            {
                var history = CallsHistory[Terminal];
                var calls = searchPredicate != null 
                    ? history.Calls.AsQueryable().Where(searchPredicate) 
                    : history.Calls.AsQueryable();
                return calls;
            }
            return null;
        }

        public void SendVoiceMessageTo(string number, string message)
        {
            FindBy(t => t.Number.Equals(number)).AcceptNotification(message);
        }

        public void EmulateState(PortState state, string number)
        {
            Ports[number].State = state;
        }

        private (decimal outgoing, decimal incoming) Count(TimeSpan callDuration)
        {
            var outgoingCallCost = TariffPlan.OutgoingCalls * callDuration.Minutes;
            var incomingCallCost = TariffPlan.IncomingCalls * callDuration.Minutes;

            return (outgoingCallCost, incomingCallCost);
        }

        private TimeSpan TurnOnCounter()
        {
            return TimeSpan.FromMinutes(new Random().Next(0, 30));
        }

        private DateTime RandomDay()
        {
            var gen = new Random();
            var start = new DateTime(2020, 12, 1);
            var range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }
    }
}