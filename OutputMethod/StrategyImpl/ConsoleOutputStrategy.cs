using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoHardwareMonitor.OutputMethod
{
    public class ConsoleOutputStrategy : IOutputStrategy
    {
        public void SendMessages(List<Util.IMessage> messages)
        {
            messages.ForEach(m => Console.WriteLine(m.GetMessage()));
            Console.WriteLine("_____________________");
        }
    }
}
