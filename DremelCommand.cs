using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ideabuilder_status
{
    internal class DremelCommand
    {
        private string _ipAddress;

        public DremelCommand()
        {
            _ipAddress = "";
        }

        public DremelCommand(string ipAddress)
        {
            _ipAddress = ipAddress;
        }

        public Dictionary<string,string> SendCommand(string command)
        {
            Uri u = new Uri("http://" + _ipAddress + "/command");

            HttpContent c = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>(command, ""),
            });


            var t = Task.Run(() => GetPrinterResponse(u, c));
            try
            {
                t.Wait();
            } 
            catch (Exception ex)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<Dictionary<string, string>>(t.Result);
        }
                
        static async Task<string> GetPrinterResponse(Uri u, HttpContent c)
        {
            var response = string.Empty;
            using (var client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0,0,1);
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
                }
            }
            return response;
        }
    }
}
