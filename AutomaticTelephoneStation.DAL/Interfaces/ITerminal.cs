using AutomaticTelephoneStation.DAL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AutomaticTelephoneStation.DAL.Interfaces
{
    public interface ITerminal
    {
        event EventHandler Plugging;
        event EventHandler Unplugging;
        event EventHandler<IContact> Accepting;
        event EventHandler Dropping;
        string Number { get; }
        IStation Operator { get; }
        IEnumerable<IContact> Contacts { get; }
        ISubscriber Owner { get; }

        IContact FindBy(Expression<Func<IContact, bool>> searchPredicate);

        void Plug();

        void Unplug();

        void Call(IContact contact);

        void TryAcceptCall(string number);

        void DropCall();

        bool TryAddContact(IContact contact);

        void AcceptedCallHandler(object calledParty, IContact caller);

        void PluggingHandler(object sender, EventArgs args);
    }
}
