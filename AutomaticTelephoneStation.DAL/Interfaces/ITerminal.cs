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
        event EventHandler<IContact> Dropping;
        event EventHandler<string> Notification;
        string Number { get; }
        ActiveCall ActiveCall { get; set; }
        IStation Operator { get; }
        IEnumerable<IContact> Contacts { get; }
        ISubscriber Owner { get; }

        IContact FindBy(Expression<Func<IContact, bool>> searchPredicate);

        void Plug();

        void Unplug();

        void Call(IContact contact);

        void TryAcceptCall(string number);

        bool TryAddContact(IContact contact);

        void AcceptedCallHandler(object calledParty, IContact caller);

        void DroppedCallHandler(object calledParty, IContact caller);

        void PluggingHandler(object sender, EventArgs args);

        void NotificationHandler(object sender, string message);

        void FinishConversation();

        void PrintCalls(Expression<Func<CallDetails, bool>> searchPredicate);

        void AcceptNotification(string message);
    }
}
