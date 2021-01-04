using AutomaticTelephoneStation.DAL.Abstract;
using AutomaticTelephoneStation.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AutomaticTelephoneStation.DAL
{
    public partial class TelecomOperator : IStation
    {
        public event EventHandler<(string, string)> ConnectionRequest;


        private ICollection<IContract> _contracts;

        public Dictionary<string, IPort> Ports { get; set; }

        public IEnumerable<IContract> Contracts => _contracts;

        public Dictionary<ITerminal, CallLog> CallsHistory { get; set; }

        public TelecomOperator()
        {
            _contracts = new List<IContract>();
            Ports = new Dictionary<string, IPort>();
            CallsHistory = new Dictionary<ITerminal, CallLog>();
        }

        protected virtual void OnConnectionRequest(object sender, string calledNumber, string caller)
        {
            ConnectionRequest?.Invoke(sender, (calledNumber, caller));
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

        public void CollectCallInformation(object calledParty, IContact caller)
        {
            var cost = Count();

            var callDetailsForCaller = new CallDetails
            {
                Beginning = DateTime.Now,
                CallType = CallTypes.Outgoing,
                Contact = caller,
                Cost = cost.outgoing,
                Duration = cost.duration
            };

            var callDetailsForCalledParty = new CallDetails
            {
                Beginning = callDetailsForCaller.Beginning,
                CallType = CallTypes.Incoming,
                Contact = caller,
                Cost = cost.outgoing,
                Duration = callDetailsForCaller.Duration
            };

            var callerTerminal = FindBy(t => t.Number.Equals(caller.Number));
            var calledTerminal = (ITerminal)calledParty;

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

        private (decimal outgoing, decimal incoming, TimeSpan duration) Count()
        {
            var callDuration = TimeSpan.FromMinutes(new Random().Next(0, 15));
            var outgoingCallCost = TariffPlan.OutgoingCalls * callDuration.Minutes;
            var incomingCallCost = TariffPlan.IncomingCalls * callDuration.Minutes;

            return (outgoingCallCost, incomingCallCost, callDuration);
        }

        public void TryConnectTo(string calledNumber, string caller)
        {
            OnConnectionRequest(this, calledNumber, caller);
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
    }
}