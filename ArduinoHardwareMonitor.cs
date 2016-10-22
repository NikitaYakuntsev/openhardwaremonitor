using ArduinoHardwareMonitor.OutputMethod;
using ArduinoHardwareMonitor.Util;
using ArduinoHardwareMonitor.Util.MessageImpl;
using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArduinoHardwareMonitor
{
    public class ArduinoHardwareMonitor
    {
        private static Identifier CPU_LOAD_IDENTIFIER = new Identifier(new string[]{"intelcpu","0", "load", "0"});
        private static Identifier CPU_TEMP_IDENTIFIER = new Identifier(new string[]{"intelcpu","0","temperature","4"});
        private static Identifier GPU_TEMP_IDENTIFIER = new Identifier(new string[]{"nvidiagpu","0","temperature","0"});
        private static Identifier GPU_FAN_IDENTIFIER = new Identifier(new string[]{"nvidiagpu","0","control","0"});
        private static Identifier MEMORY_USED_IDENTIFIER = new Identifier(new string[]{"ram","load","0"});

        private Computer _computer;
        private IHardware _cpu;
        private IHardware _gpu;
        private IHardware _memory;
        private OutputContext _context;

        public ArduinoHardwareMonitor()
        {
            Init();
        }

        public void Start()
        {            
            ISensor gpuFan = _gpu.Sensors.FirstOrDefault(s => GPU_FAN_IDENTIFIER.Equals(s.Identifier));
            ISensor gpuTemp = _gpu.Sensors.FirstOrDefault(s => GPU_TEMP_IDENTIFIER.Equals(s.Identifier));

            while (true)
            {
                List<IMessage> sessionResult = new List<IMessage>();

                this._gpu.Update();
                sessionResult.Add(new SensorValueMessage("GPU Temp, C:\t{0}", gpuTemp.Value));
                sessionResult.Add(new SensorValueMessage("GPU Fan, %:\t{0}", gpuFan.Value));

                _context.ExecuteOutputStrategy(sessionResult);

                Thread.Sleep(1000);
            }
        }

        public void Stop()
        {
            this._computer.Close();
        }

        private void Init()
        {
            this._computer = new Computer();
            this._computer.CPUEnabled = true;
            this._computer.GPUEnabled = true;
            this._computer.RAMEnabled = true;
            this._computer.Open();
            this._cpu = _computer.Hardware.First(h => HardwareType.CPU.Equals(h.HardwareType));
            this._gpu = _computer.Hardware.First(h => HardwareType.GpuNvidia.Equals(h.HardwareType));
            this._memory = _computer.Hardware.First(h => HardwareType.RAM.Equals(h.HardwareType));

            this._context = new OutputContext(new ConsoleOutputStrategy());
        }
    }
}
