using System;

namespace paradigm_shift_csharp
{
    class BatteryChecker
    {
        public enum BreachType { None, TooLow, TooHigh }

        static BreachType CheckTemperature(float temperature) =>
            temperature < 0 ? BreachType.TooLow :
            temperature > 45 ? BreachType.TooHigh :
            BreachType.None;

        static BreachType CheckSoc(float soc) =>
            soc < 20 ? BreachType.TooLow :
            soc > 80 ? BreachType.TooHigh :
            BreachType.None;


        // No lower bound for ChargeRate
        static BreachType CheckChargeRate(float chargeRate) =>
            chargeRate > 0.8f ? BreachType.TooHigh : BreachType.None;

        // Keep the original signature
        static bool batteryIsOk(float temperature, float soc, float chargeRate)
        {
            return CheckTemperature(temperature) == BreachType.None &&
                   CheckSoc(soc) == BreachType.None &&
                   CheckChargeRate(chargeRate) == BreachType.None;
        }

        static void Expect(bool actual, bool expected)
        {
            if (actual != expected)
            {
                Console.WriteLine($"Test failed! Expected: {expected}, Got: {actual}");
                Environment.Exit(1);
            }
        }

        static int Main()
        {
            Expect(batteryIsOk(25, 70, 0.7f), true);
            Expect(batteryIsOk(50, 85, 0.0f), false);
            Expect(batteryIsOk(0, 70, 0.7f), true);
            Expect(batteryIsOk(45, 70, 0.7f), true);
            Expect(batteryIsOk(-0.1f, 70, 0.7f), false);
            Expect(batteryIsOk(45.1f, 70, 0.7f), false);

            Expect(batteryIsOk(25, 20, 0.7f), true);
            Expect(batteryIsOk(25, 80, 0.7f), true);
            Expect(batteryIsOk(25, 19.9f, 0.7f), false);
            Expect(batteryIsOk(25, 80.1f, 0.7f), false);

            Expect(batteryIsOk(25, 70, 0.8f), true);
            Expect(batteryIsOk(25, 70, 0.81f), false);

            Console.WriteLine("All tests passed!");
            return 0;
        }
    }
}


