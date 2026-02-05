namespace PicoPlaca
{
    public class Restrictions
    {
        public int LastDigit { get; }
        public TimeOnly Time { get; }
        public DayOfWeek Day { get; }
        public string Restriction { get; set; }
        private static readonly Dictionary<DayOfWeek, int[]>
            RestrictedDigitsByDay = new()
            {
                [DayOfWeek.Monday] = new[] { 1, 2 },
                [DayOfWeek.Tuesday] = new[] { 3, 4 },
                [DayOfWeek.Wednesday] = new[] { 5, 6 },
                [DayOfWeek.Thursday] = new[] { 7, 8 },
                [DayOfWeek.Friday] = new[] { 9, 0 }
            };

        public Restrictions(int lastDigit, DayOfWeek day, TimeOnly time)
        {
            LastDigit = lastDigit;
            Time = time;
            Day = day;
            if (day == DayOfWeek.Saturday || day == DayOfWeek.Sunday)
            {
                Restriction = "No restrictions today, you CAN circulate freely";
                return;
            }
            if (!IsRestrictedByTime(time))
            {
                Restriction = "The vehicle CAN circulate at this moment regardless of the license plate";
                return;
            }
            else
            {
                if (IsRestrictedByPlate(day, LastDigit))
                {
                    Restriction = "The vehicle CANNOT circulate today";
                }
                else
                {
                    Restriction = "The vehicle CAN circulate today";
                }
            }
        }

        bool IsRestrictedByPlate(DayOfWeek day, int lastDigit)
        {
            return RestrictedDigitsByDay.TryGetValue(day, out var digits) && digits.Contains(lastDigit);
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
