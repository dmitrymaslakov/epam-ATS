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
            Operator.CollectCallInformation(calledParty, caller);
        }

        public void PluggingHandler(object sender, EventArgs args)
        {
            var terminal = (ITerminal)sender;
            Operator.Ports[terminal.Number].State = PortState.Connected;
            MessagePrinter.PrintToConsole($"{terminal.Owner.GetFullName()}, ваш телефон включен");
        }
    }
}
