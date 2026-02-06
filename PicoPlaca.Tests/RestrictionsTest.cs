using PicoPlaca.Services;

namespace PicoPlaca.Tests
{
    public class RestrictionsTests
    {
        private readonly Restrictions _restrictions;
        private readonly RestrictionsConfig _config;

        public RestrictionsTests()
        {
            _config = new RestrictionsConfig();
            _restrictions = new Restrictions(_config);
        }

        // ---------- WEEKENDS ----------
        [Theory]
        [InlineData(DayOfWeek.Saturday)]
        [InlineData(DayOfWeek.Sunday)]
        public void Weekend_ShouldAllowCirculation(DayOfWeek day)
        {
            // Act
            var (canCirculate, reason) = _restrictions.CheckRestriction(
                lastDigit: 1,
                day: day,
                time: new TimeOnly(8, 0));

            // Assert
            Assert.True(canCirculate);
            Assert.Contains("weekends", reason, StringComparison.OrdinalIgnoreCase);
        }

        // ---------- NON-RESTRICTED HOURS ----------
        [Fact]
        public void Weekday_OutsideRestrictedHours_ShouldAllowCirculation()
        {
            // Act
            var (canCirculate, reason) = _restrictions.CheckRestriction(
                lastDigit: 1,
                day: DayOfWeek.Monday,
                time: new TimeOnly(11, 0));

            // Assert
            Assert.True(canCirculate);
            Assert.Contains("outside restricted hours", reason, StringComparison.OrdinalIgnoreCase);
        }

        // ---------- RESTRICTED HOURS + RESTRICTED PLATE ----------
        [Theory]
        [InlineData(DayOfWeek.Monday, 1)]
        [InlineData(DayOfWeek.Monday, 2)]
        [InlineData(DayOfWeek.Tuesday, 3)]
        [InlineData(DayOfWeek.Tuesday, 4)]
        [InlineData(DayOfWeek.Wednesday, 5)]
        [InlineData(DayOfWeek.Wednesday, 6)]
        [InlineData(DayOfWeek.Thursday, 7)]
        [InlineData(DayOfWeek.Thursday, 8)]
        [InlineData(DayOfWeek.Friday, 9)]
        [InlineData(DayOfWeek.Friday, 0)]
        public void RestrictedTime_AndRestrictedPlate_ShouldBlockCirculation(
            DayOfWeek day,
            int lastDigit)
        {
            // Act
            var (canCirculate, reason) = _restrictions.CheckRestriction(
                lastDigit: lastDigit,
                day: day,
                time: new TimeOnly(8, 30));

            // Assert
            Assert.False(canCirculate);
            Assert.Contains("restriction applies", reason, StringComparison.OrdinalIgnoreCase);
        }

        // ---------- RESTRICTED HOURS + NON-RESTRICTED PLATE ----------
        [Theory]
        [InlineData(DayOfWeek.Monday, 3)]
        [InlineData(DayOfWeek.Tuesday, 1)]
        [InlineData(DayOfWeek.Wednesday, 8)]
        [InlineData(DayOfWeek.Thursday, 2)]
        [InlineData(DayOfWeek.Friday, 5)]
        public void RestrictedTime_ButNonRestrictedPlate_ShouldAllowCirculation(
            DayOfWeek day,
            int lastDigit)
        {
            // Act
            var (canCirculate, reason) = _restrictions.CheckRestriction(
                lastDigit: lastDigit,
                day: day,
                time: new TimeOnly(17, 0));

            // Assert
            Assert.True(canCirculate);
            Assert.Contains("not restricted", reason, StringComparison.OrdinalIgnoreCase);
        }

        // ---------- EDGE TIMES ----------
        [Theory]
        [InlineData(7, 0)]   // Start of morning restriction
        [InlineData(9, 30)]  // End of morning restriction
        [InlineData(16, 0)]  // Start of evening restriction
        [InlineData(19, 30)] // End of evening restriction
        public void EdgeTimes_ShouldBeConsideredRestricted(int hour, int minute)
        {
            // Act
            var (canCirculate, reason) = _restrictions.CheckRestriction(
                lastDigit: 1,
                day: DayOfWeek.Monday,
                time: new TimeOnly(hour, minute));

            // Assert
            Assert.False(canCirculate);
            Assert.Contains("restriction applies", reason, StringComparison.OrdinalIgnoreCase);
        }

        // ---------- JUST OUTSIDE RESTRICTED TIMES ----------
        [Theory]
        [InlineData(6, 59)]  // Just before morning restriction
        [InlineData(9, 31)]  // Just after morning restriction
        [InlineData(15, 59)] // Just before evening restriction
        [InlineData(19, 31)] // Just after evening restriction
        public void JustOutsideRestrictedTimes_ShouldAllowCirculation(int hour, int minute)
        {
            // Act
            var (canCirculate, reason) = _restrictions.CheckRestriction(
                lastDigit: 1,
                day: DayOfWeek.Monday,
                time: new TimeOnly(hour, minute));

            // Assert
            Assert.True(canCirculate);
            Assert.Contains("outside restricted hours", reason, StringComparison.OrdinalIgnoreCase);
        }

        // ---------- ALL DIGITS COVERAGE ----------
        [Theory]
        [InlineData(0, DayOfWeek.Friday)]
        [InlineData(1, DayOfWeek.Monday)]
        [InlineData(2, DayOfWeek.Monday)]
        [InlineData(3, DayOfWeek.Tuesday)]
        [InlineData(4, DayOfWeek.Tuesday)]
        [InlineData(5, DayOfWeek.Wednesday)]
        [InlineData(6, DayOfWeek.Wednesday)]
        [InlineData(7, DayOfWeek.Thursday)]
        [InlineData(8, DayOfWeek.Thursday)]
        [InlineData(9, DayOfWeek.Friday)]
        public void AllDigits_OnTheirRestrictedDay_ShouldBlock(int digit, DayOfWeek day)
        {
            // Act
            var (canCirculate, reason) = _restrictions.CheckRestriction(
                lastDigit: digit,
                day: day,
                time: new TimeOnly(8, 0));

            // Assert
            Assert.False(canCirculate);
            Assert.Contains($"Digit {digit}", reason);
        }

        // ---------- MORNING VS EVENING RESTRICTIONS ----------
        [Fact]
        public void MorningRestriction_ShouldBlock()
        {
            // Act
            var (canCirculate, reason) = _restrictions.CheckRestriction(
                lastDigit: 1,
                day: DayOfWeek.Monday,
                time: new TimeOnly(8, 0));

            // Assert
            Assert.False(canCirculate);
        }

        [Fact]
        public void EveningRestriction_ShouldBlock()
        {
            // Act
            var (canCirculate, reason) = _restrictions.CheckRestriction(
                lastDigit: 1,
                day: DayOfWeek.Monday,
                time: new TimeOnly(18, 0));

            // Assert
            Assert.False(canCirculate);
        }

        [Fact]
        public void BetweenRestrictions_ShouldAllow()
        {
            // Act
            var (canCirculate, reason) = _restrictions.CheckRestriction(
                lastDigit: 1,
                day: DayOfWeek.Monday,
                time: new TimeOnly(12, 0));

            // Assert
            Assert.True(canCirculate);
        }

    }
}