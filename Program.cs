using ArduinoHardwareMonitor.OutputMethod;
using ArduinoHardwareMonitor.Util;
using ArduinoHardwareMonitor.Util.MessageImpl;
using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoHardwareMonitor
{
    class Program
    {

        private static ArduinoHardwareMonitor mon = new ArduinoHardwareMonitor();

        public static void Main(string[] args)
        {
            mon.Start();            
        }      
    }
}
