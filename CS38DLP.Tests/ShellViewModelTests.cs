using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS38DLP.Models;
using CS38DLP.ViewModels;
using Xunit;
using Moq;

namespace CS38DLP.Tests
{
    public class ShellViewModelTests : IDisposable
    {
        ShellViewModel shellVM;
        Mock<DeviceModel> mockDevice = new Mock<DeviceModel>();

        public ShellViewModelTests()
        {
            shellVM = new ShellViewModel();
            shellVM.device = mockDevice.Object;
        }

        public void Dispose()
        {
            shellVM.Logging = "";
        }

        [Fact]
        public void InitDevice_Successful()
        {
            shellVM.InitDevice();
            Assert.Contains("38DL PLUS", shellVM.device.Name);
            Assert.Contains("38DL PLUS", shellVM.Logging);
        }

        [Fact]
        public void GageInfo_CheckReturnAndLogging()
        {           
            var cmd = "GAGEINFO?";
            mockDevice.Setup(foo => foo.Send(cmd)).Returns("OK\n");
            shellVM.GageInfo();
            Assert.Contains(cmd, shellVM.Logging);
            Assert.Contains("OK", shellVM.Logging);
        }

        [Fact]
        public void GageVersion_CheckReturnAndLogging()
        {
            var cmd = "VER?";
            mockDevice.Setup(foo => foo.Send(cmd)).Returns("OK\n");
            shellVM.GageVersion();
            Assert.Contains(cmd, shellVM.Logging);
            Assert.Contains("OK", shellVM.Logging);
        }

        [Fact]
        public void CommandUnits_CheckReturnAndLogging()
        {
            var cmd = "UNITS?";
            mockDevice.Setup(foo => foo.Send(cmd)).Returns("OK\n");
            shellVM.CommandUnits();
            Assert.Contains(cmd, shellVM.Logging);
            Assert.Contains("OK", shellVM.Logging);
        }

        [Fact]
        public void CommandVelocity_CheckReturnAndLogging()
        {
            var cmd = "VELOCITY?";
            mockDevice.Setup(foo => foo.Send(cmd)).Returns("OK\n");
            shellVM.CommandVelocity();
            Assert.Contains(cmd, shellVM.Logging);
            Assert.Contains("OK", shellVM.Logging);
        }

        [Fact]
        public void SendCommand_CheckReturnAndLogging()
        {
            shellVM.CommandString = "VELOCITY?";
            mockDevice.Setup(foo => foo.Send(shellVM.CommandString)).Returns("OK\n");
            shellVM.SendCommand();
            Assert.Contains(shellVM.CommandString, shellVM.Logging);
            Assert.Contains("OK", shellVM.Logging);
        }

        [Fact]
        public void CommandReading_CheckReturnAndLogging()
        {
            mockDevice.Setup(foo => foo.CurrentReading()).Returns(Tuple.Create("1.1", "MM"));
            shellVM.CommandReading();
            Assert.Equal("1.1", shellVM.TextCommandReading);            
        }

        [Theory]        
        [InlineData("VELOCITY?")]
        [InlineData("VER?")]
        public void Commands_AddFunction(string cmdString)
        {
            shellVM.Commands.Add(cmdString);
            Assert.Contains(cmdString, shellVM.Commands);
        }

        [Theory]
        [InlineData("VELOCITY?")]
        [InlineData("VER?")]
        public void SelectedCommand_CommandStringIsBindedToSelectedCommand(string cmd)
        {
            shellVM.CommandString = "";
            shellVM.SelectedCommand = cmd;
            Assert.Equal(cmd, shellVM.SelectedCommand);
            Assert.Equal(cmd, shellVM.CommandString);
        }

        [Theory]
        [InlineData("kg/m^3")]
        [InlineData("g/cc")]
        public void DensityUnitList_AddFunction(string unit)
        {
            shellVM.DensityUnitList.Add(unit);
            Assert.Contains(unit, shellVM.DensityUnitList);
        }

        [Fact]
        public void SelectedDensityUnit_SourceFromTestPieceDensityUnit()
        {
            var unit = "kg/m^3";
            shellVM.testPiece.density.Unit = unit;
            Assert.Equal(unit, shellVM.SelectedDensityUnit);
        }

        [Fact]
        public void SelectedDensityUnit_SetTestPieceDensityUnit()
        {
            var unit = "g/cc";
            shellVM.SelectedDensityUnit = unit;
            Assert.Equal(unit, shellVM.testPiece.density.Unit);
        }

        [Theory]
        [InlineData("mm/us")]
        [InlineData("inch/us")]
        public void VelocityUnitList_AddFunction(string unit)
        {
            shellVM.VelocityUnitList.Add(unit);
            Assert.Contains(unit, shellVM.VelocityUnitList);
        }

        [Fact]
        public void SelectedLongitudinalVelocityUnit_SourceFromTestPieceLongitudinalVelocityUnit()
        {
            var unit = "mm/us";
            shellVM.testPiece.longitudinalVelocity.Unit = unit;
            Assert.Equal(unit, shellVM.SelectedLongitudinalVelocityUnit);
        }

        [Fact]
        public void SelectedLongitudinalVelocityUnit_SetTestPieceLongitudinalVelocityyUnit()
        {
            var unit = "inch/us";
            shellVM.SelectedLongitudinalVelocityUnit = unit;
            Assert.Equal(unit, shellVM.testPiece.longitudinalVelocity.Unit);
        }

        [Fact]
        public void SelectedShearVelocityUnit_SourceFromTestPieceShearVelocityyUnit()
        {
            var unit = "mm/us";
            shellVM.testPiece.shearVelocity.Unit = unit;
            Assert.Equal(unit, shellVM.SelectedShearVelocityUnit);
        }

        [Fact]
        public void SelectedShearVelocityUnit_SetTestPieceShearVelocityUnit()
        {
            var unit = "inch/us";
            shellVM.SelectedShearVelocityUnit = unit;
            Assert.Equal(unit, shellVM.testPiece.shearVelocity.Unit);
        }

        [Fact]
        public void TextCommandReading_SetAndGetShouldMatch()
        {
            var reading = "1.1";
            shellVM.TextCommandReading = reading;
            Assert.Equal(reading, shellVM.TextCommandReading);
        }

        [Fact]
        public void CommandString_SetAndGetShouldMatch()
        {
            var cmd = "VER?";
            shellVM.CommandString = cmd;
            Assert.Equal(cmd, shellVM.CommandString);
        }

        [Fact]
        public void Logging_SetAndGetShouldMatch()
        {
            var log = "logging";
            shellVM.Logging = log;
            Assert.Equal(log, shellVM.Logging);
        }

        [Theory]
        [InlineData("1.1")]
        [InlineData("0")]
        [InlineData("-1.1")]
        [InlineData("abc")]
        public void DensityDigits_SetAndGetShouldMatch(string inputString)
        {
            shellVM.DensityDigits = inputString;
            Assert.Equal(inputString, shellVM.DensityDigits);
        }

        [Theory]
        [InlineData("1.1")]
        [InlineData("0")]
        [InlineData("-2")]
        public void DensityDigits_SetValidValue(string inputString)
        {
            shellVM.DensityDigits = inputString;            
            Assert.Equal(Convert.ToDouble(inputString), shellVM.testPiece.density.Digits);
        }

        [Theory]
        [InlineData("abc")]
        public void DensityDigits_SetInvalidValue(string inputString)
        {
            double value = 1.1;
            shellVM.testPiece.density.Digits = value;
            shellVM.DensityDigits = inputString;
            Assert.Equal(value, shellVM.testPiece.density.Digits);
        }

        [Theory]
        [InlineData("1.1")]
        [InlineData("0")]
        [InlineData("-1.1")]
        [InlineData("abc")]
        public void LongitudinalVelocityDigits_SetAndGetShouldMatch(string inputString)
        {
            shellVM.LongitudinalVelocityDigits = inputString;
            Assert.Equal(inputString, shellVM.LongitudinalVelocityDigits);
        }

        [Theory]
        [InlineData("1.1")]
        [InlineData("0")]
        [InlineData("-2")]
        public void LongitudinalVelocityDigits_SetValidValue(string inputString)
        {
            shellVM.LongitudinalVelocityDigits = inputString;
            Assert.Equal(Convert.ToDouble(inputString), shellVM.testPiece.longitudinalVelocity.Digits);
        }

        [Theory]
        [InlineData("abc")]
        public void LongitudinalVelocityDigits_SetInvalidValue(string inputString)
        {
            double value = 1.1;
            shellVM.testPiece.longitudinalVelocity.Digits = value;
            shellVM.LongitudinalVelocityDigits = inputString;
            Assert.Equal(value, shellVM.testPiece.longitudinalVelocity.Digits);
        }

        [Theory]
        [InlineData("1.1")]
        [InlineData("0")]
        [InlineData("-1.1")]
        [InlineData("abc")]
        public void ShearVelocityDigits_SetAndGetShouldMatch(string inputString)
        {
            shellVM.ShearVelocityDigits = inputString;
            Assert.Equal(inputString, shellVM.ShearVelocityDigits);
        }

        [Theory]
        [InlineData("1.1")]
        [InlineData("0")]
        [InlineData("-2")]
        public void ShearVelocityDigits_SetValidValue(string inputString)
        {
            shellVM.ShearVelocityDigits = inputString;
            Assert.Equal(Convert.ToDouble(inputString), shellVM.testPiece.shearVelocity.Digits);
        }

        [Theory]
        [InlineData("abc")]
        public void ShearVelocityDigits_SetInvalidValue(string inputString)
        {
            double value = 1.1;
            shellVM.testPiece.shearVelocity.Digits = value;
            shellVM.ShearVelocityDigits = inputString;
            Assert.Equal(value, shellVM.testPiece.shearVelocity.Digits);
        }

        [Theory]
        [InlineData(8000, 5.6, 2.8)]
        public void PoissonsRatio_SourceFromTestPiecePoissonsRatio(double density, double vl, double vs)
        {
            shellVM.testPiece.density.Digits = density;
            shellVM.testPiece.longitudinalVelocity.Digits = vl;
            shellVM.testPiece.shearVelocity.Digits = vs;
            Assert.Equal(shellVM.testPiece.PoissonsRatio.ToString(), shellVM.PoissonsRatio);
        }

        [Theory]
        [InlineData(8000, 5.6, 2.8)]
        public void YoungsModulus_SourceFromTestPieceYoungsModulus(double density, double vl, double vs)
        {
            shellVM.testPiece.density.Digits = density;
            shellVM.testPiece.longitudinalVelocity.Digits = vl;
            shellVM.testPiece.shearVelocity.Digits = vs;
            Assert.Equal(shellVM.testPiece.YoungsModulus.ToString(), shellVM.YoungsModulus);
        }

        [Theory]
        [InlineData(8000, 5.6, 2.8)]
        public void ShearModulus_SourceFromTestPieceShearModulus(double density, double vl, double vs)
        {
            shellVM.testPiece.density.Digits = density;
            shellVM.testPiece.longitudinalVelocity.Digits = vl;
            shellVM.testPiece.shearVelocity.Digits = vs;
            Assert.Equal(shellVM.testPiece.ShearModulus.ToString(), shellVM.ShearModulus);
        }

        [Fact]
        public void MeasureLongitudinalVelocity_SetLongitudinalVelocityDigits()
        {
            mockDevice.Setup(foo => foo.CurrentReading()).Returns(Tuple.Create("1.1", "MM"));
            shellVM.MeasureLongitudinalVelocity();
            Assert.Equal("1.1", shellVM.LongitudinalVelocityDigits);
        }

        [Theory]
        [InlineData("MM", "mm/us")]
        [InlineData("IN", "inch/us")]
        public void MeasureLongitudinalVelocity_SetSelectedLongitudinalVelocityUnit(string value, string expected)
        {
            mockDevice.Setup(foo => foo.CurrentReading()).Returns(Tuple.Create("1.1", value));
            shellVM.MeasureLongitudinalVelocity();
            Assert.Equal(expected, shellVM.SelectedLongitudinalVelocityUnit);
        }

        [Fact]
        public void MeasureShearVelocity_SetShearVelocityDigits()
        {
            mockDevice.Setup(foo => foo.CurrentReading()).Returns(Tuple.Create("1.1", "MM"));
            shellVM.MeasureShearVelocity();
            Assert.Equal("1.1", shellVM.ShearVelocityDigits);
        }

        [Theory]
        [InlineData("MM", "mm/us")]
        [InlineData("IN", "inch/us")]
        public void MeasureShearVelocity_SetSelectedShearVelocityUnit(string value, string expected)
        {
            mockDevice.Setup(foo => foo.CurrentReading()).Returns(Tuple.Create("1.1", value));
            shellVM.MeasureShearVelocity();
            Assert.Equal(expected, shellVM.SelectedShearVelocityUnit);
        }
    }
}
