using ArduinoHardwareMonitor.OutputMethod;
using ArduinoHardwareMonitor.Util;
using ArduinoHardwareMonitor.Util.MessageImpl;
using OpenHardwareMonitor.Hardware;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ArduinoHardwareMonitor
{
    public class ArduinoHardwareMonitor
    {
        private static Identifier CPU_LOAD_IDENTIFIER = new Identifier(new string[]{"intelcpu","0", "load", "0"});
        private static Identifier CPU_TEMP_IDENTIFIER = new Identifier(new string[]{"intelcpu","0","temperature","4"});
        private static Identifier GPU_TEMP_IDENTIFIER = new Identifier(new string[]{"nvidiagpu","0","temperature","0"});
        private static Identifier GPU_LOAD_IDENTIFIER = new Identifier(new string[]{"nvidiagpu","0","load","0"});
        private static Identifier GPU_FAN_IDENTIFIER = new Identifier(new string[]{"nvidiagpu","0","control","0"});
        private static Identifier MEMORY_USED_IDENTIFIER = new Identifier(new string[]{"ram","load","0"});

        private static int MEASURE_DELAY = 500; //Todo configuration selection.
        //add groups:
        //0: cpu + ram
        //1: cpu + gpu
        //2: gpu fan + temp
        //configure each parameter with indicator type (where do u like to show on)
        //button will just increment the values and (inc)%(nmaxgroups) = group to show
        //you can configure such groups (sens1 + sens2) and indicator type in ui.

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
            ISensor gpuTemp = _gpu.Sensors.FirstOrDefault(s => GPU_LOAD_IDENTIFIER.Equals(s.Identifier));
            ISensor gpuLoad = _gpu.Sensors.FirstOrDefault(s => GPU_LOAD_IDENTIFIER.Equals(s.Identifier));

            ISensor cpuLoad = _cpu.Sensors.FirstOrDefault(s => CPU_LOAD_IDENTIFIER.Equals(s.Identifier));
            ISensor cpuTemp = _cpu.Sensors.FirstOrDefault(s => CPU_TEMP_IDENTIFIER.Equals(s.Identifier));

            ISensor ramSensor = _memory.Sensors.FirstOrDefault(s => MEMORY_USED_IDENTIFIER.Equals(s.Identifier));

            while (true)
            {
                List<IMessage> sessionResult = new List<IMessage>();

                this._gpu.Update();
                sessionResult.Add(new SensorValueMessage(gpuTemp, Indicator.Indicator300, (int) GROUP.GPU));
                sessionResult.Add(new SensorValueMessage(gpuFan, Indicator.Indicator100, (int)GROUP.GPU));

                this._memory.Update();
                sessionResult.Add(new SensorValueMessage(ramSensor, Indicator.Indicator300, (int) GROUP.CPURAM));

                this._cpu.Update();
                sessionResult.Add(new SensorValueMessage(cpuLoad, Indicator.Indicator100, (int) GROUP.CPURAM));
                
                sessionResult.Add(new SensorValueMessage(cpuLoad, Indicator.Indicator100, (int) GROUP.CPUGPU));
                sessionResult.Add(new SensorValueMessage(gpuLoad, Indicator.Indicator300, (int) GROUP.CPUGPU));
                
                _context.Strategy = new ConsoleOutputStrategy();
                _context.ExecuteOutputStrategy(sessionResult);
                _context.Strategy = new SerialOutputStrategy();
                _context.ExecuteOutputStrategy(sessionResult);

                Thread.Sleep(MEASURE_DELAY); //TODO Replace with scheduler.
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

            this._context = new OutputContext(new SerialOutputStrategy());
        }

        private enum GROUP
        {
            CPURAM = 0,
            CPUGPU = 1,
            GPU = 2
        }
    }
}
