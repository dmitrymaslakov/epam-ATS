using AutomaticTelephoneStation.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomaticTelephoneStation.DAL
{
    public partial class TelecomOperator
    {
        public void ConnectionRequestHandler(object sender, (string calledNumber, string caller) arg)
        {
            switch (Ports[arg.calledNumber].State)
            {
                case PortState.Disconnected:
                    
                    break;
                case PortState.Connected:
                    Ports[arg.calledNumber].State = PortState.Busy;
                    FindBy(t => t.Number.Equals(arg.calledNumber))?.TryAcceptCall(arg.caller);
                    break;
                case PortState.Busy:
                    break;
            }

        }
    }
}
