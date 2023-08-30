using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ideabuilder_status
{
    internal class DremelInfo
    {
        private Dictionary<string, string> _info;
        private Dictionary<string, string> _infoNames;
        private string _deviceName;

        public Dictionary<string, string> Info { get { return _info; } }

        public string DeviceName { get { return _deviceName; } }

        public DremelInfo(Dictionary<string, string> _items)
        {
            _info = new Dictionary<string, string>();
            _infoNames = InitializeNames();
            _deviceName = string.Empty;
            if (_items != null)
            {
                foreach (var item in _items)
                {
                    switch (item.Key)
                    {
                        case "machine_type":
                            _deviceName = item.Value;
                            break;
                        case "ethernet_connected" or "wifi_connected":
                            _info.Add(_infoNames[item.Key], item.Value == "0" ? "No" : "Yes");
                            break;
                        case "ethernet_ip":
                            if (_items["ethernet_connected"] == "1") _info.Add(_infoNames[item.Key], item.Value);
                            break;
                        case "wifi_ip":
                            if (_items["wifi_ip"] == "1") _info.Add(_infoNames[item.Key], item.Value);
                            break;
                        case "message" or "error_code":
                            break;
                        default:
                            if (_infoNames.ContainsKey(item.Key)) _info.Add(_infoNames[item.Key], item.Value);
                            else _info.Add("'" + item.Key + "'", item.Value);
                            break;
                    }
                }
            }
        }

        private Dictionary<string, string> InitializeNames()
        {
            return new Dictionary<string, string>
            {
                { "SN", "Serial No."},
                { "api_version", "API Version" },
                { "error_code", "Error Code" },
                { "ethernet_connected", "Ethernet Connected" },
                { "ethernet_ip", "Ethernet IP Address" },
                { "firmware_version", "Firmware Version" },
                { "machine_type", "Machine Type" },
                { "message", "Message" },
                { "wifi_connected", "WiFi Connected" },
                { "wifi_ip", "WiFi IP Address" }
            };
        }
    }
}
