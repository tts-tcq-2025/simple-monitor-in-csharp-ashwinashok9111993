namespace paradigm_shift_csharp
{
    class BatteryChecker
    {
        public enum BreachType { None, TooLow, TooHigh }

        public class BatteryCheckResult
        {
            public bool IsOk { get; set; }
            public string Parameter { get; set; }
            public BreachType Breach { get; set; }
            public float Value { get; set; }
        }

        static BatteryCheckResult CheckBattery(float temperature, float soc, float chargeRate)
        {
            var tempBreach = CheckRange(temperature, 0, 45);
            if (tempBreach != BreachType.None)
                return new BatteryCheckResult { IsOk = false, Parameter = "Temperature", Breach = tempBreach, Value = temperature };

            var socBreach = CheckRange(soc, 20, 80);
            if (socBreach != BreachType.None)
                return new BatteryCheckResult { IsOk = false, Parameter = "State of Charge", Breach = socBreach, Value = soc };

            var chargeBreach = CheckRange(chargeRate, 0, 0.8f);
            if (chargeBreach != BreachType.None)
                return new BatteryCheckResult { IsOk = false, Parameter = "Charge Rate", Breach = chargeBreach, Value = chargeRate };

            return new BatteryCheckResult { IsOk = true };
        }

        static BreachType CheckRange(float value, float min, float max)
        {
            if (value < min) return BreachType.TooLow;
            if (value > max) return BreachType.TooHigh;
            return BreachType.None;
        }

        static void Expect(bool expression, bool expectation)
        {
            if (expression != expectation)
            {
                Console.WriteLine("Expected {0}, but got {1}", expectation, expression);
                Environment.Exit(1);
            }
        }

        static int Main()
        {
            Expect(CheckBattery(25, 70, 0.7f).IsOk, true);
            Expect(CheckBattery(50, 85, 0.0f).IsOk, false);
            Expect(CheckBattery(0, 70, 0.7f).IsOk, true);      // Temperature at lower bound
            Expect(CheckBattery(45, 70, 0.7f).IsOk, true);     // Temperature at upper bound
            Expect(CheckBattery(-0.1f, 70, 0.7f).IsOk, false); // Temperature just below lower bound
            Expect(CheckBattery(45.1f, 70, 0.7f).IsOk, false); // Temperature just above upper bound

            Expect(CheckBattery(25, 20, 0.7f).IsOk, true);     // SOC at lower bound
            Expect(CheckBattery(25, 80, 0.7f).IsOk, true);     // SOC at upper bound
            Expect(CheckBattery(25, 19.9f, 0.7f).IsOk, false); // SOC just below lower bound
            Expect(CheckBattery(25, 80.1f, 0.7f).IsOk, false); // SOC just above upper bound

            Expect(CheckBattery(25, 70, 0.8f).IsOk, true);     // Charge rate at upper bound
            Expect(CheckBattery(25, 70, 0.81f).IsOk, false);   // Charge rate just above upper bound
            Expect(CheckBattery(25, 70, -0.1f).IsOk, false);   // Charge rate just below lower bound
            Console.WriteLine("All ok");
            return 0;
        }
    }
}


