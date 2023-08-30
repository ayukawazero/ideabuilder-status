using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ideabuilder_status
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _ipAddress;
        private string _status;
        private PeriodicTimer _refreshTimer;

        public MainWindow()
        {
            
            InitializeComponent();

            if (Properties.Settings.Default.PrinterAddress != null)
            {
                _ipAddress = Properties.Settings.Default.PrinterAddress;
                Refresh();
                GetInfo();
            } else
            {
                _ipAddress = "";
            }
            if (Properties.Settings.Default.PollFrequency > 0)
            {
                _ = RunInBackground(TimeSpan.FromSeconds(Properties.Settings.Default.PollFrequency), () => Refresh());
            }
            _status = "";

            System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();
            ni.Icon = ApplicationResources.dicon_tray_icon;
            ni.Visible = true;
            
            ni.DoubleClick += new EventHandler(ni_DoubleClick);
        }

        private void ni_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = WindowState.Normal;
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized && Properties.Settings.Default.MinimizeToTray == true)
                this.Hide();

            base.OnStateChanged(e);
        }

        async Task RunInBackground(TimeSpan ts, Action a)
        {
            _refreshTimer = new PeriodicTimer(ts);
            while (await _refreshTimer.WaitForNextTickAsync())
            {
                a();
            }

        }
        private void GetInfo()
        {
            DremelCommand dc = new DremelCommand(_ipAddress);
            DremelInfo di = new DremelInfo(dc.SendCommand("GETPRINTERINFO"));
            lblPrinter.Content = di.DeviceName;
            if (di.Info.Count > 0)
            {
                dgInfo.ItemsSource = di.Info;
            } else
            {
                dgInfo.ItemsSource = null;
            }
        }

        private void ChangeStatus(string s)
        {
            _status = s;
            switch (s)
            {
                case "Paused":
                    btnCancel.IsEnabled = true;
                    
                    btnPause.Visibility = Visibility.Collapsed;
                    btnResume.Visibility = Visibility.Visible;
                    miPause.Visibility = Visibility.Collapsed;
                    miResume.Visibility = Visibility.Visible;                    
                    break;
                case "":
                    btnPause.IsEnabled = false;
                    btnCancel.IsEnabled = false;

                    btnPause.Visibility = Visibility.Visible;
                    btnResume.Visibility = Visibility.Collapsed;
                    miPause.Visibility = Visibility.Visible;
                    miResume.Visibility = Visibility.Collapsed;
                    break;
                default:
                    btnPause.IsEnabled = true;
                    btnCancel.IsEnabled = true;

                    btnPause.Visibility = Visibility.Visible;
                    btnResume.Visibility = Visibility.Collapsed;
                    miPause.Visibility = Visibility.Visible;
                    miResume.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void Refresh()
        {
            DremelCommand dc = new DremelCommand(_ipAddress);
            DremelStatus ds;
            Dictionary<string, string> response = dc.SendCommand("GETPRINTERSTATUS");
            if (response.Count > 0) {
                ds = new DremelStatus(response);
            } 
            else
            {
                ds = new DremelStatus();
                _refreshTimer?.Dispose();
            }

            if (ds.Status.Count > 0 && ds.Settings.Count > 0)
            {
                dgSettings.ItemsSource = ds.Settings;

                lblAddress.Content = _ipAddress;
                lblTemperature.Content = ds.Status["temperature"];
                lblDoor.Content = ds.Status["door_open"];
                lblStatus.Content = ds.Status["status"];
                lblProgress.Content = ds.Status["progress"];
                lblJobname.Content = ds.Status["jobname"] != "" ? ds.Status["jobname"] : "None";
                lblJobstatus.Content = ds.Status["jobstatus"] != "" ? ds.Status["jobstatus"] : "None";
                if (_status != ds.Status["jobstatus"]) ChangeStatus(ds.Status["jobstatus"]);
                lblTotalTime.Content = ds.Status["totalTime"];
                lblRemaining.Content = ds.Status["remaining"];
                lblElapsed.Content = ds.Status["elaspedtime"];
            } else {
                dgSettings.ItemsSource = null;

                lblAddress.Content = _ipAddress + " (No Response)";
                lblTemperature.Content = "N/A";
                lblDoor.Content = "N/A";
                lblStatus.Content = "N/A";
                lblProgress.Content = "N/A";
                lblJobname.Content = "N/A";
                lblJobstatus.Content = "N/A";
                lblTotalTime.Content = "N/A";
                lblRemaining.Content = "N/A";
                lblElapsed.Content = "N/A";
            }
        }
        
        private void File_Exit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Settings_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SettingsDialog sd = new SettingsDialog();
            sd.Owner = Application.Current.MainWindow;
            if (sd.ShowDialog() == true)
            {
                _ipAddress = Properties.Settings.Default.PrinterAddress;
                GetInfo();
                _refreshTimer.Dispose(); 
                _ = RunInBackground(TimeSpan.FromSeconds(Properties.Settings.Default.PollFrequency), () => Refresh());
                Refresh();

            }
        }

        private void Commands_Refresh_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Refresh();
        }

        private void Commands_Pause_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DremelCommand dc = new DremelCommand(_ipAddress);

            Dictionary<string, string> result = dc.SendCommand("PAUSE");
            if (result["message"] == "success")
            {
                Refresh(); 
            }
            else
            {
                MessageBox.Show("Unable to Pause Job");
            }
        }

        private void Commands_Resume_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DremelCommand dc = new DremelCommand(_ipAddress);

            Dictionary<string, string> result = dc.SendCommand("RESUME");
            if (result["message"] == "success")
            {
                Refresh();
            }
            else
            {
                MessageBox.Show("Unable to Resume Job");
            }
        }

        private void Commands_Cancel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DremelCommand dc = new DremelCommand(_ipAddress);

            Dictionary<string, string> result = dc.SendCommand("CANCEL");
            if (result["message"] == "success")
            {
                Refresh();
            } 
            else
            {
                MessageBox.Show("Unable to Cancel Job");
            }
        }
    }
}
