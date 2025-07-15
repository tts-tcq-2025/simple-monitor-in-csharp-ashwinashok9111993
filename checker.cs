using System;
using System.Diagnostics;

namespace paradigm_shift_csharp
{
class BatteryChecker
{
    //Using Lambda functions to separate the IO
    static bool IsTemperatureOk(float temperature) => temperature >= 0 && temperature <= 45;
    static bool IsSocOk(float soc) => soc >= 20 && soc <= 80;
    static bool IsChargeRateOk(float chargeRate) => chargeRate <= 0.8;

    static bool batteryIsOk(float temperature, float soc, float chargeRate) 
    {
      return (IsTemperatureOk(temperature) && IsSocOk(soc) && IsChargeRateOk(chargeRate));
    }

    static void Expect(bool expression, bool expectation) {
        if(expression && !expectation) {
            Console.WriteLine("Expected {0}, but got {1}",expectation,expression);
            Environment.Exit(1);
        }
    }

    static int Main() {
        Expect(batteryIsOk(25, 70, 0.7f),true);
        Expect(batteryIsOk(50, 85, 0.0f),false);
        Console.WriteLine("All ok");
        return 0;
    }
    
}
}
