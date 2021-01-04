using AutomaticTelephoneStation.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomaticTelephoneStation.DAL.Abstract
{
    public abstract class Subscriber : ISubscriber
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        protected Subscriber(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }

        public override bool Equals(object other) => Equals(other as Subscriber);

        public bool Equals(ISubscriber other)
        {
            if (this == other) return true;
            if (other == null) return false;
            if (FirstName != other.FirstName || LastName != other.LastName) return false;
            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = (int)2166136261;
                hash = (16777619 * hash) ^ (FirstName?.GetHashCode() ?? 0);
                hash = (16777619 * hash) ^ (LastName?.GetHashCode() ?? 0);
                return hash;
            }
        }

    }
}
