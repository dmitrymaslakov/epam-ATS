using AutomaticTelephoneStation.DAL.Abstract;
using AutomaticTelephoneStation.DAL.Helpers;
using AutomaticTelephoneStation.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AutomaticTelephoneStation.DAL
{
    public partial class Terminal : ITerminal
    {
        public event EventHandler Plugging;
        public event EventHandler Unplugging;
        public event EventHandler<IContact> Accepting;
        public event EventHandler Dropping;
        public string Number { get; }
        public IStation Operator { get; }
        private ICollection<IContact> _contacts;
        public IEnumerable<IContact> Contacts => _contacts;
        public ISubscriber Owner { get; }

        public Terminal(ISubscriber owner, IStation station)
        {
            Number = GetNumber();
            Owner = owner;
            _contacts = new List<IContact>();
            Operator = station;
        }

        public IContact FindBy(Expression<Func<IContact, bool>> searchPredicate)
        {
            try
            {
                return Contacts.AsQueryable().SingleOrDefault(searchPredicate);
            }
            catch
            {
                throw;
            }

        }

        public void Plug()
        {
            OnPlugging(this, null);
        }

        public void Unplug()
        {
            DropCall();
            OnUnplugging(this, null);
        }

        protected virtual void OnPlugging(object sender, EventArgs args)
        {
            Plugging?.Invoke(sender, args);
        }

        protected virtual void OnUnplugging(object sender, EventArgs args)
        {
            Unplugging?.Invoke(sender, args);
        }

        protected virtual void OnAccepting(object calledParty, IContact caller)
        {
            Accepting?.Invoke(calledParty, caller);
        }

        public void Call(IContact calledParty)
        {
            Operator.Ports[Number].State = PortState.Busy;
            Operator.TryConnectTo(calledParty.Number, Number);
        }

        public void TryAcceptCall(string number)
        {
            var caller = FindBy(c => c.Number.Equals(number));
            MessagePrinter.PrintToConsole($"Входящий звонок от {caller.GetFullName()}. Принять вызов? Да - \"y\"; Нет - \"n\"");
            var key = Console.ReadKey(true).Key.ToString().ToLower();

            switch (key)
            {
                case "y":
                    OnAccepting(this, caller);
                    break;

                case "n":                    

                default:

                    break;
            }

        }

        public void DropCall()
        {
            throw new NotImplementedException();
        }

        public bool TryAddContact(IContact contact)
        {
            if (!Contacts.Any(c => c.Equals(contact)))
            {
                _contacts.Add(contact);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void WriteOutCalls()
        {
            throw new NotImplementedException();
        }

        private string GetNumber()
        {
            var random = new Random();
            return $"+375 33 {random.Next(000, 999)} {random.Next(00, 99)} {random.Next(00, 99)}";
        }

    }
}
