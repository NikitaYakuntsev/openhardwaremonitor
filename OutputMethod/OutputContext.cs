using ArduinoHardwareMonitor.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoHardwareMonitor.OutputMethod
{
    public class OutputContext
    {
        private IOutputStrategy _strategy;

        public OutputContext(IOutputStrategy strategy)
        {
            this._strategy = strategy;
        }

        public void ExecuteOutputStrategy(List<IMessage> messages)
        {
            this._strategy.SendMessages(messages);
        }
    }
}
