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
                client.Terminal.Plugging += client.Terminal.PluggingHandler;
                client.Terminal.Plug();
            }

            stationOperator.ConnectionRequest += stationOperator.ConnectionRequestHandler;

            firstClient.Terminal.TryAddContact(
                new Contact(secondClient.FirstName, secondClient.LastName, secondClient.Terminal.Number));

            secondClient.Terminal.TryAddContact(
                new Contact(firstClient.FirstName, firstClient.LastName, firstClient.Terminal.Number));


            var calledContact = firstClient.Terminal.FindBy(c => c.GetFullName().Equals(secondClient.GetFullName()));

            firstClient.Terminal.Call(calledContact);
        }
    }
}
