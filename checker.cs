using System;
using System.Diagnostics;

namespace paradigm_shift_csharp
{
class BatteryChecker
{
    //lambda
    static bool IsTemperatureOk(float temperature) => temperature >= 0 && temperature <= 45;
    static bool IsSocOk(float soc) => soc >= 20 && soc <= 80;
    static bool IsChargeRateOk(float chargeRate) => chargeRate <= 0.8;

    static bool batteryIsOk(float temperature, float soc, float chargeRate) 
    {
      return (IsTemperatureOk(temperature) && IsSocOk(soc) && IsChargeRateOk(chargeRate));
    }

    static void ExpectTrue(bool expression) {
        if(!expression) {
            Console.WriteLine("Expected true, but got false");
            Environment.Exit(1);
        }
    }
    static void ExpectFalse(bool expression) {
        if(expression) {
            Console.WriteLine("Expected false, but got true");
            Environment.Exit(1);
        }
    }
    
    static int Main() {
        ExpectTrue(batteryIsOk(25, 70, 0.7f));
        ExpectFalse(batteryIsOk(50, 85, 0.0f));
        Console.WriteLine("All ok");
        return 0;
    }
    
}
}
