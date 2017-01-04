using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenHardwareMonitor.Hardware;

namespace ArduinoHardwareMonitor.Util.MessageImpl
{
    public class SensorValueMessage : IMessage
    {
        private string id;
        /// <summary>
        /// deprecated
        /// </summary>
        private HardwareType hwType;

        private int groupId;
        private Object value;
        private SensorType type;
        private Indicator prefIndicator;

        public SensorValueMessage(ISensor sensor, Indicator preferredIndicator, int groupId)
        {
            this.id = sensor.Identifier.ToString();
            this.hwType = sensor.Hardware.HardwareType;
            this.groupId = groupId;
            this.value = sensor.Value;
            this.type = sensor.SensorType;
            this.prefIndicator = preferredIndicator;
        }

        public SensorValueMessage Calibrate(int value = 100)
        {
            if (0 <= value && value <= 100)
            {
                this.value = value;
            }
            return this;
        }

        public string GetMessage()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string Id
        {
            get { return id; }
        }

        public HardwareType HwType
        {
            get { return hwType; }
        }

        public Indicator PrefIndicator
        {
            get { return prefIndicator; }
        }

        public object Value
        {
            get { return value; }
        }

        public SensorType Type
        {
            get { return type; }
        }

        public int GroupId
        {
            get { return groupId; }
        }
    }

    public enum Indicator
    {
        Indicator100 = 100,
        Indicator300 = 300
    };
}