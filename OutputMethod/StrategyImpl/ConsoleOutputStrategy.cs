using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ArduinoHardwareMonitor.OutputMethod
{
    public class ConsoleOutputStrategy : IOutputStrategy
    {
        public void SendMessages(List<Util.IMessage> messages)
        {
            String jsonString = JsonConvert.SerializeObject(messages);
            Console.WriteLine(jsonString);
            Console.WriteLine("_____________________");
        }
    }
}
