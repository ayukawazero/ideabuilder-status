using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace ideabuilder_status
{
    /// <summary>
    /// Interaction logic for SettingsDialog.xaml
    /// </summary>
    public partial class SettingsDialog : Window
    {
        private int _iPollSeconds;
        public SettingsDialog()
        {
            InitializeComponent();
            sldFrequency.Value = 1;
            txtIpAddress.Text = Properties.Settings.Default.PrinterAddress;
            _iPollSeconds = Properties.Settings.Default.PollFrequency;
            chkMinimizeToTray.IsChecked = Properties.Settings.Default.MinimizeToTray;
            SetSlider(_iPollSeconds);
        }

        public bool ValidateIP(string ipString)
        {
            if (String.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            byte tempForParsing;

            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateIP(txtIpAddress.Text))
            {
                Properties.Settings.Default.PrinterAddress = txtIpAddress.Text;
                Properties.Settings.Default.PollFrequency = _iPollSeconds;
                Properties.Settings.Default.MinimizeToTray = (bool)chkMinimizeToTray.IsChecked;
                Properties.Settings.Default.Save();
                this.DialogResult = true;
                this.Close();
            } else
            {
                MessageBox.Show("IP Address is not valid.");
            }
           
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void SetSlider(int v)
        {
            Dictionary<int, int> map = new Dictionary<int, int>
            {
                { 1, 1 },
                { 5, 2 },
                { 60, 3 },
                { 300,4 },
                { 1800, 5 },
                { 3600, 6 },
                { 0, 7 }
            };
            sldFrequency.Value = map[v];
        }
        private void sldFrequency_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            switch (sldFrequency.Value)
            {
                case 1:
                    _iPollSeconds = 1;
                    try { lblSliderValue.Content = "1 Second"; } catch { } 
                    break;

                case 2:
                    _iPollSeconds = 5;
                    lblSliderValue.Content = "5 Seconds";
                    break;

                case 3:
                    _iPollSeconds = 60;
                    lblSliderValue.Content = "1 Minute";
                    break;
                case 4:
                    _iPollSeconds = 300;
                    lblSliderValue.Content = "5 Minutes";
                    break;
                case 5:
                    _iPollSeconds = 1800;
                    lblSliderValue.Content = "30 Minutes";
                    break;
                case 6:
                    _iPollSeconds = 3600;
                    lblSliderValue.Content = "1 Hour";
                    break;
                case 7:
                    _iPollSeconds = 0;
                    lblSliderValue.Content = "Manually";
                    break;
                default:
                    break;

            }
        }
    }

}
