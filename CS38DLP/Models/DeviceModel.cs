using DeviceCommunications;
using PanTypes;
using System;
using System.Text.RegularExpressions;

namespace CS38DLP.Models
{
    public class DeviceModel
    {
        private Device _device;

        public virtual string Name
        {
            get { return _device.DeviceName; }
        }

        public virtual void InitDevice()
        {
            _device = new Device(EDeviceType.device_38dlp, "USB");
            _device.InitDevice();
        }

        public virtual string Send(string command)
        {
            var resp = "";
            _device.Clear();
            bool result = _device.SendCommand(command + "\r\n", out string sentcmd, false, false);
            if (!result)
            {
                return resp;
            }

            // Receive Responce from Device
            resp += _device.GetResponse("\r\n");

            // Loop to read empty all responces till ERROR
            while (!string.IsNullOrWhiteSpace(resp))
            {
                string temp = _device.GetResponse("\r\n");
                if (temp.StartsWith("ER:"))
                {
                    break;
                }
                else
                {
                    resp += temp;
                }
            }

            return resp;
        }

        public virtual Tuple<string, string> CurrentReading()
        {            
            string[] words = Regex.Split(Send("SEND=SINGLE"), @"\s+");
            return Tuple.Create(words[2], words[3]);
        }
    }
}