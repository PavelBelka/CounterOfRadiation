using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CounterOfRadiation.Model;

namespace CounterOfRadiation
{
    public class ModelView : INotifyPropertyChanged
    {
        private Installation _selectedInstallation;
        public ObservableCollection<Installation> Installations { get; set; }
        public Installation SelectInstallation
        {
            get { return _selectedInstallation; }
            set
            {
                _selectedInstallation = value;
                OnPropertyChanged("SelectedInstallation");
            }
        }

        public ModelView()
        {
            Installations = new ObservableCollection<Installation>
            {
                //new Installation {Mode = false, Channel1 = false, Channel2 = false, UD1 = 35, UD2 = 26}
            };
            //installation = new Installation();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
