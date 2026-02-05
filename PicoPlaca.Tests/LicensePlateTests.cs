using PicoPlacaDJ;

namespace PicoPlaca.Tests
{
    public class LicensePlateTests
    {
        // ---------- VALID CARS ----------

        [Theory]
        [InlineData("PBA-1234", false, 4)]
        [InlineData("ABC-0001", false, 1)]
        [InlineData("paB-9999", false, 9)] // lowercase + mixed
        public void CarPlate_ShouldBeValid_AndExtractLastDigit(
            string plate,
            bool expectedIsMotorcycle,
            int expectedLastDigit)
        {
            var licensePlate = new LicensePlate(plate);

            Assert.Equal(plate.ToUpper().Trim(), licensePlate.PlateNumber);
            Assert.Equal(expectedIsMotorcycle, licensePlate.IsMotorcycle);
            Assert.Equal(expectedLastDigit, licensePlate.LastDigit);
        }

        // ---------- VALID MOTORCYCLES ----------

        [Theory]
        [InlineData("PA-123J", true, 3)]
        [InlineData("AB-456Z", true, 6)]
        [InlineData("ab-789x", true, 9)] // lowercase
        public void MotorcyclePlate_ShouldBeValid_AndExtractLastDigit(
            string plate,
            bool expectedIsMotorcycle,
            int expectedLastDigit)
        {
            var licensePlate = new LicensePlate(plate);

            Assert.True(licensePlate.IsMotorcycle);
            Assert.Equal(expectedLastDigit, licensePlate.LastDigit);
        }

        // ---------- NORMALIZATION ----------

        [Fact]
        public void Plate_ShouldBeTrimmed_AndUppercased()
        {
            var plate = new LicensePlate("  pba-1234 ");

            Assert.Equal("PBA-1234", plate.PlateNumber);
        }

        // ---------- INVALID INPUT ----------

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void NullOrEmptyPlate_ShouldThrowArgumentNullException(string plate)
        {
            Assert.Throws<ArgumentNullException>(() => new LicensePlate(plate));
        }

        [Theory]
        [InlineData("123-ABC")]
        [InlineData("PB-12")]
        [InlineData("PBA1234")]
        [InlineData("PBA-12A4")]
        [InlineData("PB--1234")]
        public void InvalidFormat_ShouldThrowArgumentException(string plate)
        {
            Assert.Throws<ArgumentException>(() => new LicensePlate(plate));
        }

        // ---------- EDGE CASES ----------

        [Fact]
        public void MotorcyclePlate_WithLetter_ShouldBeDetectedCorrectly()
        {
            var plate = new LicensePlate("PA-123A");

            Assert.True(plate.IsMotorcycle);
            Assert.Equal(3, plate.LastDigit);
        }

        [Fact]
        public void CarPlate_ShouldNotBeDetectedAsMotorcycle()
        {
            var plate = new LicensePlate("PBA-5678");

            Assert.False(plate.IsMotorcycle);
            Assert.Equal(8, plate.LastDigit);
        }
    }
}
