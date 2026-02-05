namespace PicoPlaca
{
    public class Restrictions
    {
        public int LastDigit { get; }
        public TimeOnly Time { get; }
        public DayOfWeek Day { get; }
        public string Restriction { get; set; }
        public Restrictions(int lastDigit, DayOfWeek day, TimeOnly time)
        {

        }

    }
}
