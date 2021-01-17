using AutomaticTelephoneStation.DAL;
using AutomaticTelephoneStation.DAL.Helpers;
using System;
using System.Collections.Generic;

namespace AutomaticTelephoneStation.PL
{
    class Program
    {
        static void Main(string[] args)
        {
            var stationOperator = new TelecomOperator();
            var firstClient = new Client("Дмитрий", "Маслаков");
            var secondClient = new Client("Алиса", "Селезнева");

            foreach (var client in new List<Client> { firstClient, secondClient })
            {
                var contract = new Contract();

                if (stationOperator.CheckExistedClient(client))
                {
                    contract.TrySign(stationOperator, client);
                }
                else
                {
                    MessagePrinter.PrintToConsole(
                        $"Клиент {client.GetFullName()} уже имеет абонентский номер");
                }

                client.Terminal.Accepting += client.Terminal.AcceptedCallHandler;
                client.Terminal.Dropping += client.Terminal.DroppedCallHandler;
                client.Terminal.Notification += client.Terminal.NotificationHandler;
                client.Terminal.Plugging += client.Terminal.PluggingHandler;
                client.Terminal.Plug();
            }

            stationOperator.ConnectionRequest += stationOperator.ConnectionRequestHandler;
            stationOperator.FinishRequest += stationOperator.FinishRequestHandler;

            firstClient.Terminal.TryAddContact(
                new Contact(secondClient.FirstName, secondClient.LastName, secondClient.Terminal.Number));

            secondClient.Terminal.TryAddContact(
                new Contact(firstClient.FirstName, firstClient.LastName, firstClient.Terminal.Number));

            var calledAlisa = firstClient.Terminal.FindBy(c => c.GetFullName().Equals(secondClient.GetFullName()));
            var calledDmitry = secondClient.Terminal.FindBy(c => c.GetFullName().Equals(firstClient.GetFullName()));

            var iterationCount = 3;
            for (int i = 0; i < iterationCount; i++)
            {
                stationOperator.EmulateState(PortState.Busy, calledAlisa.Number);
                firstClient.Terminal.Call(calledAlisa);
                firstClient.Terminal.FinishConversation();
                stationOperator.EmulateState(PortState.Free, calledAlisa.Number);

                stationOperator.EmulateState(PortState.Disconnected, calledDmitry.Number);
                secondClient.Terminal.Call(calledDmitry);
                secondClient.Terminal.FinishConversation();
                stationOperator.EmulateState(PortState.Free, calledDmitry.Number);
            }
            MessagePrinter.PrintToConsole("\n");
            firstClient.Terminal.PrintCalls(null);
            MessagePrinter.PrintToConsole("\n");
            secondClient.Terminal.PrintCalls(c => c.Beginning > new DateTime(2020, 12, 15));

        }
    }
}
