using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using CS38DLP.Models;

namespace CS38DLP.Tests
{
    public class TestPieceModelTests
    {
        TestPieceAttribute attribute = new TestPieceAttribute();
        Density density = new Density();
        Thickness thickness = new Thickness();
        Velocity velocity = new Velocity();
        TestPieceModel testPiece = new TestPieceModel();

        [Theory]
        [InlineData(1.1)]
        [InlineData(-1.1)]
        public void TestPieceAttribute_SetAndGetDigitsShouldMatch(double digits)
        {
            attribute.Digits = digits;
            Assert.Equal(digits, attribute.Digits);
        }

        [Theory]
        [InlineData("mm")]
        [InlineData("m/s")]
        public void TestPieceAttribute_SetAndGetUnitShouldMatch(string unit)
        {
            attribute.Unit = unit;
            Assert.Equal(unit, attribute.Unit);
        }

        [Theory]
        [InlineData(8000, "kg/m^3", 8)]
        [InlineData(5, "g/cc", 5)]
        public void Density_CheckConvertedDigits(double value, string unit, double expected)
        {
            density.Digits = value;
            density.Unit = unit;
            Assert.Equal(expected, density.ConvertedDigits);
        }

        [Theory]
        [InlineData(3, "mm", 3)]
        [InlineData(2, "m", 2000)]
        [InlineData(1, "inch", 25.4)]
        public void Thickness_CheckConvertedDigits(double value, string unit, double expected)
        {
            thickness.Digits = value;
            thickness.Unit = unit;
            Assert.Equal(expected, thickness.ConvertedDigits);
        }

        [Theory]
        [InlineData(3, "mm/us", 3)]
        [InlineData(2000, "m/s", 2)]
        [InlineData(1, "inch/us", 25.4)]
        public void Velocity_CheckConvertedDigits(double value, string unit, double expected)
        {
            velocity.Digits = value;
            velocity.Unit = unit;
            Assert.Equal(expected, velocity.ConvertedDigits);
        }

        [Theory]
        [InlineData(5.6, 2.8, 1.0 / 3)]
        [InlineData(5.6, 5.6, 0.0)]
        [InlineData(-5.6, 2.8, 0.0)]
        [InlineData(5.6, -2.8, 0.0)]
        public void TestPieceModel_CheckPoissonsRatio(double vl, double vs, double expected)
        {
            testPiece.longitudinalVelocity.Unit = "mm/us";
            testPiece.shearVelocity.Unit = "mm/us";
            testPiece.longitudinalVelocity.Digits = vl;
            testPiece.shearVelocity.Digits = vs;
            Assert.Equal(expected, testPiece.PoissonsRatio);
        }

        [Theory]
        [InlineData(8, 5.6, 2.8, 167.2533)]
        [InlineData(8, 5.6, 5.6, 0.0)]
        [InlineData(-8, 5.6, 2.8, 0.0)]
        [InlineData(8, -5.6, 2.8, 0.0)]
        [InlineData(8, 5.6, -2.8, 0.0)]
        public void TestPieceModel_CheckYoungsModulus(double density, double vl, double vs, double expected)
        {
            testPiece.density.Unit = "g/cc";
            testPiece.longitudinalVelocity.Unit = "mm/us";
            testPiece.shearVelocity.Unit = "mm/us";
            testPiece.density.Digits = density;
            testPiece.longitudinalVelocity.Digits = vl;
            testPiece.shearVelocity.Digits = vs;
            Assert.Equal(expected, testPiece.YoungsModulus, 4);
        }

        [Theory]
        [InlineData(8, 2.8, 62.72)]
        [InlineData(-8, 2.8, 0.0)]
        [InlineData(8, -2.8, 0.0)]
        public void TestPieceModel_CheckShearModulus(double density, double vs, double expected)
        {
            testPiece.density.Unit = "g/cc";
            testPiece.shearVelocity.Unit = "mm/us";
            testPiece.density.Digits = density;
            testPiece.shearVelocity.Digits = vs;
            Assert.Equal(expected, testPiece.ShearModulus, 2);
        }
    }
}
