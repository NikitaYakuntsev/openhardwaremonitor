using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ArduinoHardwareMonitor.OutputMethod
{
    public class SerialOutputStrategy : IOutputStrategy
    {
        private static SerialPort port;

        public SerialOutputStrategy()
        {
            if (port == null)
            {
                port = new SerialPort("COM4");
            }
            if (!port.IsOpen)
            {
                port.Open();
            }
        }

        public void SendMessages(List<Util.IMessage> messages)
        {

            String jsonString = JsonConvert.SerializeObject(messages);
            if (port.IsOpen)
            {
                port.WriteLine(jsonString);
            }
        }
    }
}
