using Caliburn.Micro;
using CS38DLP.Models;
using DeviceCommunications;
using PanTypes;
using System;
using System.Text.RegularExpressions;

namespace CS38DLP.ViewModels
{
    public class ShellViewModel : Screen
    {
        private BindableCollection<CommandModel> _commands = new BindableCollection<CommandModel>();
        private DeviceModel _device;
        private CommandModel _selectedCommand;
        private string _commandString;

        private string _textCommandReading;
        private string _logging;

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

            Logging += "Please click Initialize Device button.\n";
        }

        public void InitDevice()
        {
            _device = new DeviceModel();
            Logging += String.Format("\nInitializing...Done!\nDevice Name: {0}\n", _device.Name);
        }

        public void GageInfo()
        {
            var cmd = "GAGEINFO?";
            Logging += String.Format("\n=============SEND=============\n{0}\n\n\n", cmd);
            Logging += String.Format("\n*********************RECEIVE********************\n{0}\n\n\n", _device.Send(cmd));
        }

        public void GageVersion()
        {
            var cmd = "VER?";
            Logging += String.Format("\n=============SEND=============\n{0}\n\n\n", cmd);
            Logging += String.Format("\n*********************RECEIVE********************\n{0}\n\n\n", _device.Send(cmd));
        }

        public void CommandUnits()
        {
            var cmd = "UNITS?";
            Logging += String.Format("\n=============SEND=============\n{0}\n\n\n", cmd);
            Logging += String.Format("\n*********************RECEIVE********************\n{0}\n\n\n", _device.Send(cmd));
        }

        public void CommandVelocity()
        {
            var cmd = "VELOCITY?";
            Logging += String.Format("\n=============SEND=============\n{0}\n\n\n", cmd);
            Logging += String.Format("\n*********************RECEIVE********************\n{0}\n\n\n", _device.Send(cmd));
        }

        public void SendCommand()
        {
            Logging += String.Format("\n=============SEND=============\n{0}\n\n\n", CommandString);
            Logging += String.Format("\n*********************RECEIVE********************\n{0}\n\n\n", _device.Send(CommandString));
        }

        public void CommandReading()
        {
            var readings = _device.CurrentReading();
            TextCommandReading = readings.Item1;            
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
                CommandString = _selectedCommand.Content;                
            }
        }

        public string TextCommandReading
        {
            get { return _textCommandReading; }
            set
            {
                _textCommandReading = value;
                NotifyOfPropertyChange(() => TextCommandReading);
            }
        }

        public string CommandString
        {
            get { return _commandString; }
            set
            {
                _commandString = value;
                NotifyOfPropertyChange(() => CommandString);
            }
        }

        public string Logging
        {
            get { return _logging; }
            set
            {
                _logging = value;
                NotifyOfPropertyChange(() => Logging);
            }
        }
    }
}
