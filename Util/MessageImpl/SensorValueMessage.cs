using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoHardwareMonitor.Util.MessageImpl
{
    public class SensorValueMessage : IMessage
    {
        private String _formatPattern;
        private Object _value;

        public SensorValueMessage(String pattern, Object value)
        {
            this._formatPattern = pattern;
            this._value = value;
        }

        public string GetMessage()
        {
            return String.Format(_formatPattern, _value);
        }
    }
}
