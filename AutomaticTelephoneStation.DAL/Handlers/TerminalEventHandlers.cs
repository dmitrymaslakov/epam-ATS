using AutomaticTelephoneStation.DAL.Helpers;
using AutomaticTelephoneStation.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomaticTelephoneStation.DAL
{
    public partial class Terminal
    {
        public void AcceptedCallHandler(object calledParty, IContact caller)
        {
            Operator.AddActiveCall(((ITerminal)calledParty).Number, caller.Number);
        }

        public void DroppedCallHandler(object calledParty, IContact caller)
        {
            Operator.ReturnCallParticipantsToInitialState(((ITerminal)calledParty).Number, caller.Number);
            Operator.SendVoiceMessageTo(caller.Number, $"{caller.GetFullName()}, ваш вызов сброшен");
        }

        public void PluggingHandler(object sender, EventArgs args)
        {
            var terminal = (ITerminal)sender;
            Operator.Ports[terminal.Number].State = PortState.Free;
            MessagePrinter.PrintToConsole($"{terminal.Owner.GetFullName()}, ваш телефон включен");
        }

        public void NotificationHandler(object sender, string message)
        {
            MessagePrinter.PrintToConsole(message);
        }
    }
}
