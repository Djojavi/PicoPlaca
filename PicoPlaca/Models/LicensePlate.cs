using System.Text.RegularExpressions;

namespace PicoPlaca.Models
{
    /// <summary>
    ///     License plate model and validation
    /// </summary>
    public class LicensePlate
    {
        public string PlateNumber { get; }
        public int LastDigit { get; }

        private static readonly Regex PlatePattern = new Regex(
            @"^[A-Z]{2,3}-\d{3,4}[A-Z]?$", RegexOptions.Compiled | RegexOptions.IgnoreCase //Edge cases of motorcycles
        );
        public bool IsMotorcycle { get; }

        public LicensePlate(string plateNumber)
        {
            if (string.IsNullOrEmpty(plateNumber))
            {
                throw new ArgumentNullException("Please provide a license plate");
            }
            PlateNumber = plateNumber.ToUpper().Trim(); //Estandarize to uppercase and trim white spaces

            if (!PlatePattern.IsMatch(PlateNumber))
            {
                throw new ArgumentException("Please provide a license plate with a valid format. Ex: PBA-1234 (cars), (PA-123J) motorcycles");
            }

            var parts = PlateNumber.Split('-');
            if (parts.Length != 2)
            {
                throw new ArgumentException("Please provide a license plate with a valid format. Ex: PBA-1234 (cars), (PA-123J) motorcycles");
            }

            IsMotorcycle = CheckIsMotorcycle(parts[1]);
            LastDigit = LastNumber(parts[1], IsMotorcycle);

        }

        private bool CheckIsMotorcycle(string secondPart)
        {
            var lastChar = secondPart[secondPart.Length - 1];
            if (char.IsLetter(lastChar)) { return true; }
            else { return false; }
        }

        private int LastNumber(string secondPart, bool isMotorcycle)
        {
            string lastNumber;
            if (isMotorcycle)
            {
                lastNumber = secondPart[secondPart.Length - 2].ToString();
            }
            else
            {
                lastNumber = secondPart[secondPart.Length - 1].ToString();
            }
            if (int.TryParse(lastNumber, out int number))
            {
                return number;
            }
            else
            {
                throw new ArgumentException("Coludn't obtain the last digit of the license plate");
            }
        }
    }
}
