using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoHardwareMonitor.OutputMethod
{
    public class SerialOutputStrategy : IOutputStrategy
    {
        public void SendMessages(List<Util.IMessage> messages)
        {
            throw new NotImplementedException();
        }
    }
}
