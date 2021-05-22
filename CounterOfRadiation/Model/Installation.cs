using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CounterOfRadiation.Model
{
    public class Installation : INotifyPropertyChanged
    {
        private SerialPort _comport;
        private int mode;
        private byte Ud1;
        private byte Ud2;
        private bool channel_1;
        private bool channel_2;

        public int Mode
        {
            get { return mode; }
            set 
            {
                mode = value;
                OnPropertyChanged("Mode");
            }
        }

        public byte UD1
        {
            get { return Ud1; }
            set 
            {
                Ud1 = value;
                OnPropertyChanged("Ud1");
            }
        }

        public byte UD2
        {
            get { return Ud2; }
            set
            {
                Ud2 = value;
                OnPropertyChanged("Ud2");
            }
        }

        public bool Channel1
        {
            get { return channel_1; }
            set
            {
                channel_1 = value;
                OnPropertyChanged("Channel_1");
            }
        }

        public bool Channel2
        {
            get { return channel_2; }
            set
            {
                channel_2 = value;
                OnPropertyChanged("Channel_2");
            }
        }

        public Installation(int Mode, bool Channel1, bool Channel2, byte UD1, byte UD2){
            mode = Mode;
            channel_1 = Channel1;
            channel_2 = Channel2;
            Ud1 = UD1;
            Ud2 = UD2;
        }

        public void Connect(string _com_port){
            _comport = new SerialPort(_com_port, 9600, Parity.None, 8);
            _comport.ReadTimeout = 500;
            _comport.WriteTimeout = 500;
            try{
                _comport.Open();
            }
            catch (Exception ex){
                //rtbLogger.AppendText("Ошибка:" + ex.ToString() + "\r\n");
                _comport.Close();
            }
        }

        public void Disconnect(){
            _comport.Close();
        }

        public bool IsConnect(){
            if (_comport.IsOpen)
                return true;
            else
                return false;
        }

        public void SetU1(){
            byte[] data = { 0x40, 0x01, UD1, 0x26 };
            SetByte(data);
        }

        public void SetU2(){
            byte[] data = { 0x40, 0x02, UD2, 0x26 };
            SetByte(data);
        }

        public byte[] GetU1()
        {
            byte[] data = { 0x40, 0x03, 0xFF, 0x26 };
            byte[] recive = GetByte(data);
            return recive;
        }

        public void GetU2()
        {
            byte[] data = { 0x40, 0x04, 0xFF, 0x26 };
            GetByte(data);
        }

        public void SetChannelMode(){
            byte channel = 0x13;
            switch (Mode){
                case 0:
                    channel = 0x11;
                    break;
                case 1:
                    channel = 0x12;
                    break;
                case 2:
                    channel = 0x13;
                    break;
            }
            byte[] data = { 0x40, channel, 0xFF, 0x26 };
            SetByte(data);
        }

        public void StartStop(string act){
            byte channel = 0;
            switch(act){
                case "start":
                    channel = 0x21;
                    break;
                case "stop":
                    channel = 0x22;
                    break;
                case "pause":
                    channel = 0x23;
                    break;
                case "reset":
                    channel = 0x24;
                    break;
            }
            byte[] data = { 0x40, channel, 0xFF, 0x26 };
            SetByte(data);
        }

        private byte[] GetByte(byte[] comm){
            byte[] data = new byte[4];
            SetByte(comm);
            for (int i = 0; i < 4; i++){
                data[i] = (byte)_comport.ReadByte();
            }
            return data;
        }

        private void SetByte(byte[] comm){
            _comport.DiscardInBuffer();
            _comport.DiscardOutBuffer();
            _comport.Write(comm, 0, 4);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
