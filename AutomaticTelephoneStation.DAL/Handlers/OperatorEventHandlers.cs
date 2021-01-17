using AutomaticTelephoneStation.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticTelephoneStation.DAL
{
    public partial class TelecomOperator
    {
        public void ConnectionRequestHandler(object sender, (string calledNumber, string callerNumber) arg)
        {
            var callerTerminal = FindBy(t => t.Number.Equals(arg.callerNumber));
            var calledTerminal = FindBy(t => t.Number.Equals(arg.calledNumber));

            switch (Ports[arg.calledNumber].State)
            {
                case PortState.Free:
                    Ports[arg.calledNumber].State = PortState.Busy;
                    calledTerminal?.TryAcceptCall(arg.callerNumber);
                    break;
                case PortState.Disconnected:
                    Ports[arg.callerNumber].State = PortState.Free;
                    SendVoiceMessageTo(arg.callerNumber,
                        $"{callerTerminal.Owner.GetFullName()}, абонент временно недоступен или находится вне зоны действия сети");
                    break;
                case PortState.Busy:
                    Ports[arg.callerNumber].State = PortState.Free;
                    SendVoiceMessageTo(arg.callerNumber,
                        $"{callerTerminal.Owner.GetFullName()}, номер абонента занят");
                    break;
            }

        }

        public void FinishRequestHandler(object sender, (string calledNumber, string callerNumber) arg)
        {
            CollectCallInformation(arg.calledNumber, arg.callerNumber);

            RemoveActiveCall(FindBy(t => t.Number.Equals(arg.calledNumber)).ActiveCall);

            ReturnCallParticipantsToInitialState(arg.calledNumber, arg.callerNumber);
        }

    }
}
