using AutomaticTelephoneStation.DAL.Interfaces;
using System;

namespace AutomaticTelephoneStation.DAL
{
    public class Contract : IContract
    {
        public IStation Operator { get; private set; }
        public IClient Client { get; private set; }
        public DateTime ContractDay { get; private set; }
        public ITerminal Terminal { get; private set; }

        public bool TrySign(IStation _operator, IClient client) 
        {
            Operator = _operator;
            Client = client;
            ContractDay = DateTime.Now;
            Terminal = new Terminal(client, Operator);
            if (_operator.TryAddContract(this))
            {
                client.Contract = this;
                client.Terminal = Terminal;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool Equals(object other) => Equals(other as Contract);

        public bool Equals(IContract other)
        {
            if (this == other) return true;
            if (other == null) return false;
            if (!Client.Equals(other.Client)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = (int)2166136261;
                hash = (16777619 * hash) ^ (Client?.GetHashCode() ?? 0);
                return hash;
            }
        }

    }
}
