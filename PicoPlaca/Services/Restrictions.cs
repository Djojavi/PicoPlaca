namespace PicoPlaca.Services
{
    public class Restrictions
    {
        private readonly RestrictionsConfig _config;

        public Restrictions(RestrictionsConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public (bool CanCirculate, string Reason) CheckRestriction(int lastDigit, DayOfWeek day, TimeOnly time)
        {
            if (IsWeekend(day))
            {
                return (true, "The vehicle CAN circulate today. No restrictions on weekends");
            }
            if (!IsRestrictedByTime(time))
            {
                return (true, $"The vehicle CAN circulate right now. Time {time:HH:mm} is outside restricted hours");
            }
            if (!IsRestrictedByPlate(day, lastDigit))
            {
                return (true, $"The vehicle CAN circulate today. Last digit {lastDigit} is not restricted on {day}");
            }
            return (false,
                $"The vehicle CANNOT circulate today. Pico y Placa restriction applies. Digit {lastDigit} is restricted on {day} " +
                $"during {_config.MorningStart:HH:mm}-{_config.MorningEnd:HH:mm} " +
                $"and {_config.EveningStart:HH:mm}-{_config.EveningEnd:HH:mm}");
        }

        bool IsWeekend(DayOfWeek day)
        {
            return day == DayOfWeek.Saturday || day == DayOfWeek.Sunday;
        }

        bool IsRestrictedByPlate(DayOfWeek day, int lastDigit)
        {
            if (_config.RestrictedDigitsByDay.TryGetValue(day, out var digits))
            {
                return digits.Contains(lastDigit);
            }
            return false;
        }

        static bool IsWithin(TimeOnly time, TimeOnly start, TimeOnly end)
        {
            return time >= start && time <= end;
        }

        bool IsRestrictedByTime(TimeOnly time)
        {
            var morningStart = new TimeOnly(7, 0);
            var morningEnd = new TimeOnly(9, 30);

            var eveningStart = new TimeOnly(16, 0);
            var eveningEnd = new TimeOnly(19, 30);
            return IsWithin(time, morningStart, morningEnd) || IsWithin(time, eveningStart, eveningEnd);
        }
    }
}
