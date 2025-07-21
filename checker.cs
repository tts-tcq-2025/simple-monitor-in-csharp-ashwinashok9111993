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

        private class ParameterLimit
        {
            public string Name { get; }
            public float Min { get; }
            public float Max { get; }
            public float Value { get; }

            public ParameterLimit(string name, float min, float max, float value)
            {
                Name = name;
                Min = min;
                Max = max;
                Value = value;
            }
        }

        static BatteryCheckResult CheckBattery(float temperature, float soc, float chargeRate)
        {
            var parameters = new[]
            {
                new ParameterLimit("Temperature", 0, 45, temperature),
                new ParameterLimit("State of Charge", 20, 80, soc),
                new ParameterLimit("Charge Rate", 0, 0.8f, chargeRate)
            };

            foreach (var param in parameters)
            {
                var breach = CheckRange(param.Value, param.Min, param.Max);
                if (breach != BreachType.None)
                {
                    return new BatteryCheckResult
                    {
                        IsOk = false,
                        Parameter = param.Name,
                        Breach = breach,
                        Value = param.Value
                    };
                }
            }

            return new BatteryCheckResult { IsOk = true };
        }

        static BreachType CheckRange(float value, float min, float max) =>
            value < min ? BreachType.TooLow :
            value > max ? BreachType.TooHigh :
            BreachType.None;

        static void Expect(BatteryCheckResult result, bool expectation)
        {
            if (result.IsOk == expectation)
            {
                Console.WriteLine(
                    "Test passed: Expected {0}, got {1}{2}",
                    expectation,
                    result.IsOk,
                    result.IsOk ? "" : $" (Breach: {result.Breach}, Parameter: {result.Parameter}, Value: {result.Value})"
                );
            }
            else
            {
                Console.WriteLine(
                    "Test failed: Expected {0}, but got {1} (Breach: {2}, Parameter: {3}, Value: {4})",
                    expectation,
                    result.IsOk,
                    result.Breach,
                    result.Parameter,
                    result.Value
                );
                Environment.Exit(1);
            }
        }

        static int Main()
        {
            Expect(CheckBattery(25, 70, 0.7f), true);
            Expect(CheckBattery(50, 85, 0.0f), false);
            Expect(CheckBattery(0, 70, 0.7f), true);
            Expect(CheckBattery(45, 70, 0.7f), true);
            Expect(CheckBattery(-0.1f, 70, 0.7f), false);
            Expect(CheckBattery(45.1f, 70, 0.7f), false);

            Expect(CheckBattery(25, 20, 0.7f), true);
            Expect(CheckBattery(25, 80, 0.7f), true);
            Expect(CheckBattery(25, 19.9f, 0.7f), false);
            Expect(CheckBattery(25, 80.1f, 0.7f), false);

            Expect(CheckBattery(25, 70, 0.8f), true);
            Expect(CheckBattery(25, 70, 0.81f), false);
            Expect(CheckBattery(25, 70, -0.1f), false);
            Console.WriteLine("All ok");
            return 0;
        }
    }
}


