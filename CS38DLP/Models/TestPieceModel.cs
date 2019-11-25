using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS38DLP.Models
{    
    public class TestPieceAttribute
    {
        public double Digits { get; set; }
        public string Unit { get; set; }
    }

    public class Density : TestPieceAttribute
    {
        public Density()
        {
            Digits = 0.0;
            Unit = "kg/m^3";
        }

        public double ConvertedDigits
        {
            get
            {
                double convertedDigits = 0.0;
                switch (Unit)
                {
                    case "kg/m^3":
                        convertedDigits = Digits * 1e-3;
                        break;

                    case "g/cc":
                        convertedDigits = Digits;
                        break;

                    default:
                        convertedDigits = Digits;
                        break;
                }
                return convertedDigits;
            }
        }
    }

    public class Thickness : TestPieceAttribute
    {
        public Thickness()
        {
            Digits = 0.0;
            Unit = "mm";
        }

        public double ConvertedDigits
        {
            get
            {
                double convertedDigits = 0.0;
                switch (Unit)
                {
                    case "mm":
                        convertedDigits = Digits;
                        break;

                    case "m":
                        convertedDigits = Digits * 1e3;
                        break;

                    case "inch":
                        convertedDigits = Digits * 25.4;
                        break;

                    default:
                        convertedDigits = Digits;
                        break;
                }
                return convertedDigits;
            }
        }
    }

    public class Velocity : TestPieceAttribute
    {
        public Velocity()
        {
            Digits = 0.0;
            Unit = "mm/us";
        }

        public double ConvertedDigits
        {
            get
            {
                double convertedDigits = 0.0;
                switch (Unit)
                {
                    case "mm/us":
                        convertedDigits = Digits;
                        break;

                    case "m/s":
                        convertedDigits = Digits * 1e-3;
                        break;

                    case "inch/us":
                        convertedDigits = Digits * 25.4;
                        break;

                    default:
                        convertedDigits = Digits;
                        break;
                }
                return convertedDigits;
            }
        }
    }

    public class TestPieceModel
    {
        public Density density;
        public Thickness thickness;
        public Velocity longitudinalVelocity;
        public Velocity shearVelocity;

        public TestPieceModel()
        {
            density = new Density();
            thickness = new Thickness();
            longitudinalVelocity = new Velocity();
            shearVelocity = new Velocity();
        }

        public double PoissonsRatio
        {
            get
            {
                if ((longitudinalVelocity.ConvertedDigits > 0) && (shearVelocity.ConvertedDigits > 0))
                {
                    double ratio = shearVelocity.ConvertedDigits / longitudinalVelocity.ConvertedDigits;
                    double divisor = (2 - 2 * Math.Pow(ratio, 2));
                    double dividend = (1 - 2 * Math.Pow(ratio, 2));
                    if ((divisor == 0) || (dividend == 0))
                        return 0.0;
                    else
                        return dividend / divisor;
                }
                else
                    return 0.0;
            }
        }

        public double YoungsModulus
        {
            get
            {
                var pr = PoissonsRatio;
                if ((longitudinalVelocity.ConvertedDigits > 0) && (pr > 0) && (pr < 0.5) && (density.ConvertedDigits > 0))
                    return Math.Pow(longitudinalVelocity.ConvertedDigits, 2) * density.ConvertedDigits * (1 + pr) * (1 - 2 * pr) / (1 - pr);
                else
                    return 0.0;
            }
        }

        public double ShearModulus
        {
            get
            {
                if ((shearVelocity.ConvertedDigits > 0) && (density.ConvertedDigits > 0))
                    return Math.Pow(shearVelocity.ConvertedDigits, 2) * density.ConvertedDigits;
                else
                    return 0.0;
            }
        }
    }
}
