using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CounterOfRadiation.Model
{
    public class Installation : INotifyPropertyChanged
    {
        private bool mode;
        private int Ud1;
        private int Ud2;
        private bool channel_1;
        private bool channel_2;

        public bool Mode
        {
            get { return mode; }
            set 
            {
                mode = value;
                OnPropertyChanged("Mode");
            }
        }

        public int UD1
        {
            get { return Ud1; }
            set 
            {
                Ud1 = value;
                OnPropertyChanged("Ud1");
            }
        }

        public int UD2
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

        public Installation(bool Mode, bool Channel1, bool Channel2, int UD1, int UD2)
        {
            this.mode = Mode;
            this.channel_1 = Channel1;
            this.channel_2 = Channel2;
            this.Ud1 = UD1;
            this.Ud2 = UD2;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
