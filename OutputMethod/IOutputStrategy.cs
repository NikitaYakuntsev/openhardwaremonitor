﻿using ArduinoHardwareMonitor.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoHardwareMonitor.OutputMethod
{
    public interface IOutputStrategy
    {
        void SendMessages(List<IMessage> messages);
    }
}
