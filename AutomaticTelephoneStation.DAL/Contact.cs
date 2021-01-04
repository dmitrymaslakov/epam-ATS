using AutomaticTelephoneStation.DAL.Abstract;
using AutomaticTelephoneStation.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomaticTelephoneStation.DAL
{
    public class Contact : Subscriber, IContact
    {
        public string Number { get; set; }

        public Contact(string firstName, string lastName, string number) : base(firstName, lastName)
        {
            Number = number;
        }
    }
}
