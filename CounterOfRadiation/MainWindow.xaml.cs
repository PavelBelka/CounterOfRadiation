using CounterOfRadiation.Model;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
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
        public Installation installation = new Installation(2, false, false, 35, 28);
        private Timer timer1;
        private TimeSpan current_time;
        private TimeSpan tim_time;
        private DateTime startTime;
        private string serialPort = null;
        private double koeff_volume_mass = 0.0;
        private double koeff_actiity = 0.0;
        private int flag_experiemnt = 0;
        private int impuls = 0;

        public MainWindow()
        {
            InitializeComponent();
            Initialization();        
        }

        public void Initialization()
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

            Work_mode.IsEnabled = false;
            Check_channel_1.IsEnabled = false;
            Check_channel_2.IsEnabled = false;

            TextBox_Ud1.IsEnabled = false;
            TextBox_Ud2.IsEnabled = false;
            Button_Ud1.IsEnabled = false;
            Button_Ud2.IsEnabled = false;

            TextBox_Time_Experiment.IsEnabled = false;
            Check_Time_Experiment.IsEnabled = false;
            Button_experiment.IsEnabled = false;

            TextBox_volume.Text = Convert.ToString(koeff_volume_mass);
            TextBox_specific_activity.Text = Convert.ToString(koeff_actiity);

            Lists();
        }

        public void Block_interface()
        {
            Button_connect.IsEnabled = false;
            Button_disconnect.IsEnabled = false;
            List_port.IsEnabled = false;

            Work_mode.IsEnabled = false;
            Check_channel_1.IsEnabled = false;
            Check_channel_2.IsEnabled = false;

            TextBox_Ud1.IsEnabled = false;
            TextBox_Ud2.IsEnabled = false;
            Button_Ud1.IsEnabled = false;
            Button_Ud2.IsEnabled = false;

            Button_value.IsEnabled = false;
            TextBox_volume.IsEnabled = false;
            TextBox_specific_activity.IsEnabled = false;

            TextBox_Time_Experiment.IsEnabled = false;
            Check_Time_Experiment.IsEnabled = false;
        }

        public void Unblock_interface()
        {
            Button_connect.IsEnabled = true;
            Button_disconnect.IsEnabled = true;
            List_port.IsEnabled = true;

            Work_mode.IsEnabled = true;
            Check_channel_1.IsEnabled = true;
            Check_channel_2.IsEnabled = true;

            TextBox_Ud1.IsEnabled = true;
            TextBox_Ud2.IsEnabled = true;
            Button_Ud1.IsEnabled = true;
            Button_Ud2.IsEnabled = true;

            Button_value.IsEnabled = true;
            TextBox_volume.IsEnabled = true;
            TextBox_specific_activity.IsEnabled = true;

            TextBox_Time_Experiment.IsEnabled = true;
            Check_Time_Experiment.IsEnabled = true;
        }

        public void Lists()
        {
            string[] ports = SerialPort.GetPortNames();
            string[] modes = { "Независмые каналы", "CH1+2" };
            foreach (string port in ports){
                List_port.Items.Add(port);
            }
            foreach (string mode in modes){
                Work_mode.Items.Add(mode);
            }

        }

        private void Button_connect_Click(object sender, RoutedEventArgs e)
        {
            if(serialPort != null){
                installation.Connect(serialPort);
            }
            if(installation.IsConnect()){
                Status_connect.Content = "Порт открыт";
                Status_connect.Foreground = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                Work_mode.IsEnabled = true;
                Work_mode.SelectedIndex = 0;

                TextBox_Ud1.IsEnabled = true;
                TextBox_Ud2.IsEnabled = true;
                Button_Ud1.IsEnabled = true;
                Button_Ud2.IsEnabled = true;

                TextBox_Time_Experiment.IsEnabled = true;
                Check_Time_Experiment.IsEnabled = true;

                TextBox_Ud1.Text = Convert.ToString(installation.UD1);
                installation.SetU1();
                TextBox_Ud2.Text = Convert.ToString(installation.UD2);
                installation.SetU2();
            }
        }

        private void Button_disconnect_Click(object sender, RoutedEventArgs e){
            if (installation.IsConnect()){
                installation.Disconnect();
                Initialization();
            }
        }

        private void List_port_SelectionChanged(object sender, SelectionChangedEventArgs e){
            ComboBox combo = (ComboBox)sender;
            serialPort = combo.SelectedItem.ToString();
        }

        private void Work_mode_SelectionChanged(object sender, SelectionChangedEventArgs e){
            ComboBox combo = (ComboBox)sender;
            int choice = combo.SelectedIndex;
            if(choice == 0){
                Check_channel_1.IsEnabled = true;
                Check_channel_2.IsEnabled = true;
            }
            else{
                Check_channel_1.IsEnabled = false;
                Check_channel_2.IsEnabled = false;
                installation.Mode = 2;
                installation.SetChannelMode();
                Button_experiment.IsEnabled = true;
            }
        }

        private void Check_channel_1_Click(object sender, RoutedEventArgs e){
            if ((bool)Check_channel_1.IsChecked){
                Label_Check_channel_1.Content = "Включен";
                Label_Check_channel_1.Foreground = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                installation.Mode = 0;
                installation.SetChannelMode();
                Button_experiment.IsEnabled = true;
            }
            else{
                Label_Check_channel_1.Content = "Выключен";
                Label_Check_channel_1.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
        }

        private void Check_channel_2_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)Check_channel_2.IsChecked)
            {
                Label_Check_channel_2.Content = "Включен";
                Label_Check_channel_2.Foreground = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                installation.Mode = 1;
                installation.SetChannelMode();
                Button_experiment.IsEnabled = true;
            }
            else
            {
                Label_Check_channel_2.Content = "Выключен";
                Label_Check_channel_2.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
        }

        private void Button_Ud1_Click(object sender, RoutedEventArgs e){
            installation.UD1 = Convert.ToByte(TextBox_Ud1.Text);
            installation.SetU1();
        }

        private void Button_Ud2_Click(object sender, RoutedEventArgs e){
            installation.UD2 = Convert.ToByte(TextBox_Ud1.Text);
            installation.SetU2();
        }

        private void Button_value_Click(object sender, RoutedEventArgs e){
            koeff_volume_mass = Convert.ToDouble(TextBox_volume.Text, System.Globalization.CultureInfo.InvariantCulture);
            koeff_actiity = Convert.ToDouble(TextBox_specific_activity.Text, System.Globalization.CultureInfo.InvariantCulture);
        }

        private void Button_experiment_Click(object sender, RoutedEventArgs e){
            if (flag_experiemnt == 0){
                Label_Status_Experiment.Content = "Начат экcперимент";
                Label_Status_Experiment.Foreground = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                Button_experiment.Content = "Стоп";
                flag_experiemnt = 1;
                impuls = 0;
                Block_interface();
                if ((bool)Check_Time_Experiment.IsChecked){
                    tim_time = TimeSpan.Parse(TextBox_Time_Experiment.Text);
                    startTime = DateTime.Now;
                    TimerCallback tm = new TimerCallback(Interview2);
                    timer1 = new Timer(tm, null, 0, 100);
                }
                else{
                    startTime = DateTime.Now;
                    TimerCallback tm = new TimerCallback(Interview1);
                    timer1 = new Timer(tm, null, 0, 100);
                }
            }
            else{
                timer1.Dispose();
                timer1 = null;
                Unblock_interface();
                Label_Status_Experiment.Content = "Экcперимент завершен";
                Label_Status_Experiment.Foreground = new SolidColorBrush(Color.FromRgb(200, 0, 0));
                Button_experiment.Content = "Старт";
                flag_experiemnt = 0;
            }         
        }

        private void Interview1(object obj){
            current_time = DateTime.Now.Subtract(startTime);
            impuls += 1;
            Dispatcher.BeginInvoke(new Action(() =>
            {
                TextBox_sum_impulses.Text = Convert.ToString(impuls);
                TextBox_Time_Experiment.Text = Convert.ToString(current_time);
            }), null);
        }

        private void Interview2(object obj)
        {
            current_time = tim_time - DateTime.Now.Subtract(startTime);
            if (current_time <= TimeSpan.Zero){
                timer1.Dispose();
                timer1 = null;
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    Unblock_interface();
                    Label_Status_Experiment.Content = "Экcперимент завершен";
                    Label_Status_Experiment.Foreground = new SolidColorBrush(Color.FromRgb(200, 0, 0));
                    Button_experiment.Content = "Старт";
                    flag_experiemnt = 0;
                }), null);
            }
            impuls += 1;
            Dispatcher.BeginInvoke(new Action(() =>
            {
                TextBox_sum_impulses.Text = Convert.ToString(impuls);
                TextBox_Time_Experiment.Text = Convert.ToString(current_time);
            }), null);
        }
    }
}
