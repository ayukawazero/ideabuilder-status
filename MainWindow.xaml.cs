using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Text;
using System.Text.Json.Serialization;
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

namespace ideabuilder_status
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        struct Setting
        {
            string key;
            string value;
        }
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Uri u = new Uri("http://192.168.1.32/command");
            
            HttpContent c  = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("GETPRINTERSTATUS", ""),
            });


            var t = Task.Run(() => GetPrinterStatus(u, c));
            t.Wait();
            //MessageBox.Show(t.Result);

            Dictionary<string, string> jsonResult = JsonConvert.DeserializeObject<Dictionary<string, string>>(t.Result);
            dgSettings.ItemsSource = jsonResult;


        }
        static async Task<string> GetPrinterStatus(Uri u, HttpContent c)
        {
            var response = string.Empty;
            using (var client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = u,
                    Content = c
                };

                HttpResponseMessage result = await client.SendAsync(request);
                if (result.IsSuccessStatusCode)
                {
                    response = await result.Content.ReadAsStringAsync();
                } else { 
                    MessageBox.Show(result.ToString()); 
                }
            }
            return response;
        }
    }
}
