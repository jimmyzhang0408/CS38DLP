using Caliburn.Micro;
using CS38DLP.Models;
using DeviceCommunications;
using PanTypes;
using System;

namespace CS38DLP.ViewModels
{
    public class ShellViewModel : Screen
    {
        private BindableCollection<CommandModel> _commands = new BindableCollection<CommandModel>();
        private DeviceModel _device;
        private CommandModel _selectedCommand;
        private string _textSendCommand;
        private string _textCommandReading;
        private string _logging = "";

        public ShellViewModel()
        {
            Commands.Add(new CommandModel { Content = "FTPINFO?" });
            Commands.Add(new CommandModel { Content = "GAGEINFO?" });
            Commands.Add(new CommandModel { Content = "FILEDIR?" });
            Commands.Add(new CommandModel { Content = "APPSUDIR" });
            Commands.Add(new CommandModel { Content = "XDCRLIST?" });
            Commands.Add(new CommandModel { Content = "MEMORY?" });
            Commands.Add(new CommandModel { Content = "SEND=SINGLE" });
            Commands.Add(new CommandModel { Content = "SEND=FILE" });
            Commands.Add(new CommandModel { Content = "VER?" });
            Commands.Add(new CommandModel { Content = "UNITS?" });
            Commands.Add(new CommandModel { Content = "VELOCITY?" });
            Commands.Add(new CommandModel { Content = "MODE?" });
            Commands.Add(new CommandModel { Content = "DATAWIN1?" });
            Commands.Add(new CommandModel { Content = "SRATE?" });
            Commands.Add(new CommandModel { Content = "WFGRAB?" });
            Commands.Add(new CommandModel { Content = "BATTLEVEL?" });
            Commands.Add(new CommandModel { Content = "VELOCITY=value" });
            Commands.Add(new CommandModel { Content = "MONITOR=GO" });
            Commands.Add(new CommandModel { Content = "PROTO=SINGLE" });
            Commands.Add(new CommandModel { Content = "SETUPNAME?" });
            Commands.Add(new CommandModel { Content = "SETUPNAME=name" });          
        }

        public void InitDevice()
        {
            _device = new DeviceModel();
            var name = _device.Name;
            _logging += "Device Name: " + _device.Name + Environment.NewLine;
        }

        public void GageInfo()
        {
            _device.Send("GAGEINFO?");
        }

        public void GageVersion()
        {
            _device.Send("VER?");
        }

        public void CommandUnits()
        {
            _device.Send("UNITS?");
        }

        public void CommandVelocity()
        {
            _device.Send("VELOCITY?");
        }

        public void SendCommand()
        {
            
        }

        public void CommandReading()
        {

        }

        public BindableCollection<CommandModel> Commands
        {
            get { return _commands; }
            set { _commands = value; }
        }

        public CommandModel SelectedCommand
        {
            get { return _selectedCommand; }
            set
            {
                _selectedCommand = value;
                NotifyOfPropertyChange(() => SelectedCommand);
            }
        }

        public string TextCommandReading
        {
            get { return _textCommandReading; }
            set { _textCommandReading = value; }
        }

        public string TextSendCommand
        {
            get { return _textSendCommand; }
            set { _textSendCommand = value; }
        }

        public string Logging
        {
            get { return _logging; }
            set { _logging = value; }
        }
    }
}
