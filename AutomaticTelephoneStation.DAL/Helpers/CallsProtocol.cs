using System;
using System.Collections.Generic;
using System.Text;

namespace AutomaticTelephoneStation.DAL.Helpers
{
    public class CallsProtocol
    {
        public IEnumerable<CallDetails> Calls { get; set; }
        public CallsProtocol(IEnumerable<CallDetails> calls)
        {
            Calls = calls;
        }

        public void Get()
        {
            var offset = 12;
            var columnStart = 0;
            var columns = new List<string> { "Абонент", "Тип звонка", "Дата, время", "Продолжительность", "Стоимость" };
            var columnsStart = new List<int>();

            foreach (var column in columns)
            {
                columnsStart.Add(columnStart);
                Console.SetCursorPosition(columnStart, Console.CursorTop);
                Console.Write(column);
                columnStart = columnStart + column.Length + offset;
            }

            Console.WriteLine($"\n");

            foreach (var call in Calls)
            {
                Console.SetCursorPosition(columnsStart[0], Console.CursorTop);
                Console.Write(call.Contact.GetFullName());
                Console.SetCursorPosition(columnsStart[1], Console.CursorTop);
                Console.Write(call.CallType.ToString());
                Console.SetCursorPosition(columnsStart[2], Console.CursorTop);
                Console.Write(call.Beginning);
                Console.SetCursorPosition(columnsStart[3], Console.CursorTop);
                Console.Write(call.Duration);
                Console.SetCursorPosition(columnsStart[4], Console.CursorTop);
                Console.WriteLine(call.Cost);
            }
        }

    }
}
