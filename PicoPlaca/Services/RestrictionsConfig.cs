namespace PicoPlaca.Services
{
    public class RestrictionsConfig
    {
        public Dictionary<DayOfWeek, int[]> RestrictedDigitsByDay { get; }
        public TimeOnly MorningStart { get; }
        public TimeOnly MorningEnd { get; }
        public TimeOnly EveningStart { get; }
        public TimeOnly EveningEnd { get; }

        public RestrictionsConfig()
        {
            RestrictedDigitsByDay = new Dictionary<DayOfWeek, int[]>
            {
                [DayOfWeek.Monday] = new[] { 1, 2 },
                [DayOfWeek.Tuesday] = new[] { 3, 4 },
                [DayOfWeek.Wednesday] = new[] { 5, 6 },
                [DayOfWeek.Thursday] = new[] { 7, 8 },
                [DayOfWeek.Friday] = new[] { 9, 0 }
            };
            MorningStart = new TimeOnly(7, 0);
            MorningEnd = new TimeOnly(9, 30);
            EveningStart = new TimeOnly(16, 0);
            EveningEnd = new TimeOnly(19, 30);
        }

    }
}
