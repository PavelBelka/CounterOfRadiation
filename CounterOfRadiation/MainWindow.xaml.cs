using CounterOfRadiation.Model;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CounterOfRadiation
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Installation installation = new Installation(false, false, false, 35, 28);
        private SerialPort serialPort;

        public MainWindow()
        {
            InitializeComponent();
            initialization();        
        }

        public void initialization()
        {
            Status_connect.Content = "Порт не открыт";
            Status_connect.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));

            Status_mode.Content = "Неизвестно";
            Status_mode.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));

            Label_Check_channel_1.Content = "Выключен";
            Label_Check_channel_1.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));

            Label_Check_channel_2.Content = "Выключен";
            Label_Check_channel_2.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));

            Label_Status_Experiment.Content = "Не готов";
            Label_Status_Experiment.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));

            listPorts();
        }

        public void listPorts()
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                List_port.Items.Add(port);
            }
        }
    }
}
