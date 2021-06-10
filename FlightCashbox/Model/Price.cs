using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightCashbox.Model
{
    class Price
    {
        public static int Get(string TypeTrain, string TypeRailwayCarriage, bool Upper, bool Side)
        {
            switch (TypeRailwayCarriage)
            {
                case "Сидячий":
                    return 900;
                case "Плацкарт":
                    if (Upper)
                    {
                        if (Side) return 1350;
                        return 1450;
                    }
                    else if (Side) return 1400;
                    else return 1500;
                case "Купе":
                    if (Upper) return 2400;
                    else return 2500;
                case "Люкс":
                    return 3300;
                default:
                    return 0;
            }
        }
    }
}
