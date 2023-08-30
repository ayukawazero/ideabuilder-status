using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ideabuilder_status
{
    internal class DremelStatus
    {
        private Dictionary<string, string> _settings;
        private Dictionary<string, string> _status;
        private Dictionary<string, string> _settingNames;

        public Dictionary<string, string> Settings { get { return _settings; } }
        public Dictionary<string, string> Status { get { return _status; } }

        public DremelStatus()
        {
            _settings = new Dictionary<string, string>();
            _status = new Dictionary<string, string>();
            _settingNames = InitializeNames();
        }

        public DremelStatus(Dictionary<string, string> _items)
        {
            _settings = new Dictionary<string, string>();
            _status = new Dictionary<string, string>();
            TimeSpan t;
            _settingNames = InitializeNames();
            if (_items != null)
            {
                foreach (var item in _items)
                {
                    switch (item.Key)
                    {
                        case "jobname" or "status":
                            if (item.Value.Length > 1) _status.Add(item.Key, char.ToUpper(item.Value[0]) + item.Value[1..]);
                            else _status.Add(item.Key, item.Value);
                            break;
                        case "jobstatus":
                            if (item.Value == "!pausing") _status.Add(item.Key, "Paused");
                            else if (item.Value == "!resuming") _status.Add(item.Key, "Resuming");
                            else if (item.Value.Length > 1) _status.Add(item.Key, char.ToUpper(item.Value[0]) + item.Value[1..]);
                            else _status.Add(item.Key, item.Value);
                            break;
                        case "temperature":
                            _status.Add(item.Key, string.Format("{0}°C", item.Value));
                            break;
                        case "progress":
                            _status.Add(item.Key, string.Format("{0}%", item.Value));
                            break;
                        case "totalTime" or "elaspedtime":
                            t = TimeSpan.FromSeconds(Int32.Parse(item.Value));
                            _status.Add(item.Key, string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds));
                            break;
                        case "remaining":
                            t = TimeSpan.FromSeconds(Int32.Parse(item.Value));
                            _status.Add(item.Key, string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds));
                            t = TimeSpan.FromSeconds(Int32.Parse(_items["totalTime"]) - Int32.Parse(_items["elaspedtime"]));
                            _status.Add("realremaining", string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds));
                            break;

                        case "door_open":
                            _status.Add(item.Key, item.Value == "0" ? "Closed" : "Open");
                            break;
                        case "networkBuild":
                            _status.Add(item.Key, item.Value == "0" ? "No" : "Yes");
                            break;
                        case "message" or "error_code":
                            break;
                        case "buildPlate_target_temperature" or "chamber_temperature" or "extruder_target_temperature" or "platform_temperature" or "temperature":
                            _settings.Add(_settingNames[item.Key], string.Format("{0}°C", item.Value));
                            break;
                        default:
                            if (_settingNames.ContainsKey(item.Key)) _settings.Add(_settingNames[item.Key], item.Value);
                            else _settings.Add("'" + item.Key + "'", item.Value);
                            break;
                    }
                }
            }
        }

        private Dictionary<string, string> InitializeNames()
        {
            return new Dictionary<string, string>
            {
                { "buildPlate_target_temperature", "Buildplate Target Temp"},
                { "chamber_temperature", "Chamber Temp" },
                { "door_open", "Door Open" }, // nonsetting
                { "elaspedtime", "Elapsed Time" }, //nonsetting
                { "error_code", "Error Code" },
                { "extruder_target_temperature", "Extruder Target Temp" },
                { "fanSpeed", "Fan Speed" },
                { "filament_type ", "Filament Type" },
                { "firmware_version", "Firmware Version" },
                { "jobname", "Job Name" },
                { "jobstatus", "Job Status" },
                { "layer", "Layer" }, // ???
                { "message", "Message" },
                { "networkBuild", "Network Build" },
                { "platform_temperature", "Platform Temp" },
                { "progress", "Progress" },
                { "remaining", "Time Remaining" },
                { "status", "Printer Status" },
                { "temperature", "Extruder Temperature" },
                { "totalTime", "Estimated Build Time" } //newsetting
            };
        }
    }
}