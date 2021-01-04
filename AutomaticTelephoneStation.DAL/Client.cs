using AutomaticTelephoneStation.DAL.Abstract;
using AutomaticTelephoneStation.DAL.Helpers;
using AutomaticTelephoneStation.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace AutomaticTelephoneStation.DAL
{
    public class Client : Subscriber, IClient
    {
        private IContract _contract;
        public IContract Contract
        {
            get => _contract;
            set
            {
                if (_contract != null)
                {
                    MessagePrinter.PrintToConsole($"{GetFullName()}, вы уже имеете абонентский номер");
                }
                else
                {
                    _contract = value;
                }  
            }
        } 
        public ITerminal Terminal { get; set; }

        public Client(string firstName, string lastName) : base(firstName, lastName)
        {

        }
    }
}
