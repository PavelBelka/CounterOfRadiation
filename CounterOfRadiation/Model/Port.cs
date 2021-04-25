using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CounterOfRadiation.Model
{
    class Port
    {
        SerialPort serialPort = new SerialPort();

        public Port()
        {
            this.serialPort.BaudRate = 9600;
            this.serialPort.Parity = Parity.None;
            this.serialPort.DataBits = 8;
            this.serialPort.StopBits = StopBits.One;
            this.serialPort.Handshake = Handshake.None;
        }

        public void Connect(string name)
        {
            this.serialPort.PortName = name;

        }
    }
}
