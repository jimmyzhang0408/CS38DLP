using Caliburn.Micro;
using CS38DLP.Models;
using DeviceCommunications;
using PanTypes;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CS38DLP.ViewModels
{
    public class ShellViewModel : Screen
    {
        private IWindowManager _manager = new WindowManager();
        private BindableCollection<string> _commands = new BindableCollection<string>();
        private BindableCollection<string> _densityUnitList = new BindableCollection<string>();
        //private BindableCollection<string> _thicknessUnitList = new BindableCollection<string>();
        private BindableCollection<string> _velociyUnit = new BindableCollection<string>();

        private string _selectedCommand;
        private string _selectedDensityUnit;
        //private string _selectedThicknessUnit;
        private string _selectedLongitudinalVelocityUnit;
        private string _selectedShearVelocityUnit;
        
        private DeviceModel _device;
                
        private string _commandString;
        private string _textCommandReading;
        private string _logging;

        private TestPieceModel testPiece = new TestPieceModel();
        private string _densityDigits;
        //private string _thicknessDigits;
        private string _longitudianlVelocityDigits;
        private string _shearVelocityDigits;        

        public ShellViewModel()
        {
            Commands.Add("FTPINFO?");
            Commands.Add("GAGEINFO?");
            Commands.Add("FILEDIR?");
            Commands.Add("APPSUDIR");
            Commands.Add("XDCRLIST?");
            Commands.Add("MEMORY?");
            Commands.Add("SEND=SINGLE");
            Commands.Add("SEND=FILE");
            Commands.Add("VER?");
            Commands.Add("UNITS?");
            Commands.Add("VELOCITY?");
            Commands.Add("MODE?");
            Commands.Add("DATAWIN1?");
            Commands.Add("SRATE?");
            Commands.Add("WFGRAB?");
            Commands.Add("BATTLEVEL?");
            Commands.Add("VELOCITY=value");
            Commands.Add("MONITOR=GO");
            Commands.Add("PROTO=SINGLE");
            Commands.Add("SETUPNAME?");
            Commands.Add("SETUPNAME=name");

            DensityUnitList.Add("kg/m^3");
            DensityUnitList.Add("g/cc");

            //ThicknessUnitList.Add("mm");
            //ThicknessUnitList.Add("m");
            //ThicknessUnitList.Add("inch");

            VelocityUnitList.Add("mm/us");
            VelocityUnitList.Add("m/s");
            VelocityUnitList.Add("inch/us");

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

        public BindableCollection<string> Commands
        {
            get { return _commands; }
            set { _commands = value; }
        }

        public string SelectedCommand
        {
            get { return _selectedCommand; }
            set
            {
                _selectedCommand = value;
                NotifyOfPropertyChange(() => SelectedCommand);
                CommandString = _selectedCommand;                
            }
        }
        
        public BindableCollection<string> DensityUnitList
        {
            get { return _densityUnitList; }
            set { _densityUnitList = value; }
        }
        
        public string SelectedDensityUnit
        {
            get
            {
                _selectedDensityUnit = testPiece.density.Unit;
                return _selectedDensityUnit;
            }
            set
            {
                _selectedDensityUnit = value;
                NotifyOfPropertyChange(() => SelectedDensityUnit);
                testPiece.density.Unit = _selectedDensityUnit;
                NotifyOfPropertyChange(() => PoissonsRatio);
                NotifyOfPropertyChange(() => YoungsModulus);
                NotifyOfPropertyChange(() => ShearModulus);
            }
        }

        //public BindableCollection<string> ThicknessUnitList
        //{
        //    get { return _thicknessUnitList; }
        //    set { _thicknessUnitList = value; }
        //}

        //public string SelectedThicknessUnit
        //{
        //    get
        //    {
        //        _selectedThicknessUnit = testPiece.thickness.Unit;
        //        return _selectedThicknessUnit;
        //    }
        //    set
        //    {
        //        _selectedThicknessUnit = value;
        //        NotifyOfPropertyChange(() => SelectedThicknessUnit);
        //        testPiece.thickness.Unit = _selectedThicknessUnit;
        //        NotifyOfPropertyChange(() => PoissonsRatio);
        //        NotifyOfPropertyChange(() => YoungsModulus);
        //        NotifyOfPropertyChange(() => ShearModulus);
        //    }
        //}

        public BindableCollection<string> VelocityUnitList
        {
            get { return _velociyUnit; }
            set { _velociyUnit = value; }
        }
        
        public string SelectedLongitudinalVelocityUnit
        {
            get
            {
                _selectedLongitudinalVelocityUnit = testPiece.longitudinalVelocity.Unit;
                return _selectedLongitudinalVelocityUnit;
            }
            set
            {
                _selectedLongitudinalVelocityUnit = value;
                NotifyOfPropertyChange(() => SelectedLongitudinalVelocityUnit);
                testPiece.longitudinalVelocity.Unit = _selectedLongitudinalVelocityUnit;
                NotifyOfPropertyChange(() => PoissonsRatio);
                NotifyOfPropertyChange(() => YoungsModulus);
                NotifyOfPropertyChange(() => ShearModulus);
            }
        }
        
        public string SelectedShearVelocityUnit
        {
            get
            {
                _selectedShearVelocityUnit = testPiece.shearVelocity.Unit;
                return _selectedShearVelocityUnit;
            }
            set
            {
                _selectedShearVelocityUnit = value;
                NotifyOfPropertyChange(() => SelectedShearVelocityUnit);
                testPiece.shearVelocity.Unit = _selectedShearVelocityUnit;
                NotifyOfPropertyChange(() => PoissonsRatio);
                NotifyOfPropertyChange(() => YoungsModulus);
                NotifyOfPropertyChange(() => ShearModulus);
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

        public void DisplayFormula()
        {
            FormulaViewModel formulaView = new FormulaViewModel();
            _manager.ShowWindow(formulaView);
        }

        public string DensityDigits
        {
            get { return _densityDigits; }
            set
            {
                _densityDigits = value;
                NotifyOfPropertyChange(() => DensityDigits);

                if (Double.TryParse(value, out double outValue))
                {
                    testPiece.density.Digits = outValue;
                    NotifyOfPropertyChange(() => PoissonsRatio);
                    NotifyOfPropertyChange(() => YoungsModulus);
                    NotifyOfPropertyChange(() => ShearModulus);
                }
            }
        }
               
        //public string ThicknessDigits
        //{
        //    get { return _thicknessDigits; }
        //    set
        //    {
        //        _thicknessDigits = value;
        //        NotifyOfPropertyChange(() => ThicknessDigits);

        //        if (Double.TryParse(value, out double outValue))
        //        {
        //            testPiece.thickness.Digits = outValue;
        //            NotifyOfPropertyChange(() => PoissonsRatio);
        //            NotifyOfPropertyChange(() => YoungsModulus);
        //            NotifyOfPropertyChange(() => ShearModulus);
        //        }
        //    }
        //}

        public string LongitudinalVelocityDigits
        {
            get { return _longitudianlVelocityDigits; }
            set
            {
                _longitudianlVelocityDigits = value;
                NotifyOfPropertyChange(() => LongitudinalVelocityDigits);

                if (Double.TryParse(value, out double outValue))
                {
                    testPiece.longitudinalVelocity.Digits = outValue;                    
                    NotifyOfPropertyChange(() => PoissonsRatio);
                    NotifyOfPropertyChange(() => YoungsModulus);
                    NotifyOfPropertyChange(() => ShearModulus);
                }
            }
        }

        public string ShearVelocityDigits
        {
            get { return _shearVelocityDigits; }
            set
            {
                _shearVelocityDigits = value;
                NotifyOfPropertyChange(() => ShearVelocityDigits);

                if (Double.TryParse(value, out double outValue))
                {
                    testPiece.shearVelocity.Digits = outValue;                    
                    NotifyOfPropertyChange(() => PoissonsRatio);
                    NotifyOfPropertyChange(() => YoungsModulus);
                    NotifyOfPropertyChange(() => ShearModulus);
                }
            }
        }

        public string PoissonsRatio
        {
            get { return testPiece.PoissonsRatio.ToString(); }
        }

        public string YoungsModulus
        {
            get { return testPiece.YoungsModulus.ToString(); }
        }

        public string ShearModulus
        {
            get { return testPiece.ShearModulus.ToString(); }
        }

        public void MeasureLongitudinalVelocity()
        {
            var readings = _device.CurrentReading();
            LongitudinalVelocityDigits = readings.Item1;
            switch (readings.Item2)
            {
                case "MM":
                    SelectedLongitudinalVelocityUnit = "mm/us";
                    break;

                case "IN":
                    SelectedLongitudinalVelocityUnit = "inch/us";
                    break;

                default:
                    SelectedLongitudinalVelocityUnit = "mm/us";
                    break;
            }              
        }

        public void MeasureShearVelocity()
        {
            var readings = _device.CurrentReading();
            ShearVelocityDigits = readings.Item1;
            switch (readings.Item2)
            {
                case "MM":
                    SelectedShearVelocityUnit = "mm/us";
                    break;

                case "IN":
                    SelectedShearVelocityUnit = "inch/us";
                    break;

                default:
                    SelectedShearVelocityUnit = "mm/us";
                    break;
            }
        }
    }
}
