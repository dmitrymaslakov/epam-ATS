using System;
using System.Collections.Generic;
using System.Text;

namespace AutomaticTelephoneStation.DAL.Helpers
{
    public class CallProtocol
    {
        public CallDetails Call { get; set; }
        public CallProtocol(CallDetails call)
        {
            Call = call;
        }

        public void Get()
        {

            Console.SetCursorPosition(30, Console.CursorTop);

            Console.WriteLine("Тип звонка");

            Console.WriteLine($"\n");
        }

    }
}
