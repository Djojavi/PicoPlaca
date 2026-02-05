namespace PicoPlaca.Tests
{
    public class RestrictionsTests
    {
        // ---------- WEEKENDS ----------

        [Theory]
        [InlineData(DayOfWeek.Saturday)]
        [InlineData(DayOfWeek.Sunday)]
        public void Weekend_ShouldAllowCirculation(
            DayOfWeek day)
        {
            var restrictions = new Restrictions(
                lastDigit: 1,
                day: day,
                time: new TimeOnly(8, 0));

            Assert.Equal(
                "No restrictions today, you CAN circulate freely",
                restrictions.Restriction);
        }

        // ---------- NON-RESTRICTED HOURS ----------

        [Fact]
        public void Weekday_OutsideRestrictedHours_ShouldAllowCirculation()
        {
            var restrictions = new Restrictions(
                lastDigit: 1,
                day: DayOfWeek.Monday,
                time: new TimeOnly(11, 0));

            Assert.Equal(
                "The vehicle CAN circulate at this moment regardless of the license plate",
                restrictions.Restriction);
        }

        // ---------- RESTRICTED HOURS + RESTRICTED PLATE ----------

        [Theory]
        [InlineData(DayOfWeek.Monday, 1)]
        [InlineData(DayOfWeek.Monday, 2)]
        [InlineData(DayOfWeek.Tuesday, 3)]
        [InlineData(DayOfWeek.Tuesday, 4)]
        [InlineData(DayOfWeek.Friday, 9)]
        [InlineData(DayOfWeek.Friday, 0)]
        public void RestrictedTime_AndRestrictedPlate_ShouldBlockCirculation(
            DayOfWeek day,
            int lastDigit)
        {
            var restrictions = new Restrictions(
                lastDigit: lastDigit,
                day: day,
                time: new TimeOnly(8, 30));

            Assert.Equal(
                "The vehicle CANNOT circulate today",
                restrictions.Restriction);
        }

        // ---------- RESTRICTED HOURS + NON-RESTRICTED PLATE ----------

        [Theory]
        [InlineData(DayOfWeek.Monday, 3)]
        [InlineData(DayOfWeek.Tuesday, 1)]
        [InlineData(DayOfWeek.Wednesday, 8)]
        [InlineData(DayOfWeek.Thursday, 2)]
        public void RestrictedTime_ButNonRestrictedPlate_ShouldAllowCirculation(
            DayOfWeek day,
            int lastDigit)
        {
            var restrictions = new Restrictions(
                lastDigit: lastDigit,
                day: day,
                time: new TimeOnly(17, 0));

            Assert.Equal(
                "The vehicle CAN circulate today",
                restrictions.Restriction);
        }

        // ---------- EDGE TIMES ----------

        [Theory]
        [InlineData(7, 0)]
        [InlineData(9, 30)]
        [InlineData(16, 0)]
        [InlineData(19, 30)]
        public void EdgeTimes_ShouldBeConsideredRestricted(
            int hour,
            int minute)
        {
            var restrictions = new Restrictions(
                lastDigit: 1,
                day: DayOfWeek.Monday,
                time: new TimeOnly(hour, minute));

            Assert.Equal(
                "The vehicle CANNOT circulate today",
                restrictions.Restriction);
        }
    }
}

