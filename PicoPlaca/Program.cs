using PicoPlaca.Models;
using PicoPlaca.Services;
using System.Globalization;

namespace PicoYPlaca
{
    /// <summary>
    /// Console application for Pico y Placa prediction
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===========================================");
            Console.WriteLine("   Pico y Placa Predictor - 2026");
            Console.WriteLine("===========================================");
            Console.WriteLine();

            while (true)
            {
                Console.WriteLine("Enter a valid license plate. Ex: PBX-123 or 'exit' to stop:");
                var licensePlate = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(licensePlate) ||
                        licensePlate.Trim().ToLower() == "exit")
                {
                    Console.WriteLine("\nThank you for using Pico y Placa Predictor!");
                    break;
                }

                var licensePlateObj = new LicensePlate(licensePlate);
                int lastDigit = licensePlateObj.LastDigit;

                Console.Write("Enter date (yyyy-MM-dd): ");
                var dateConsole = Console.ReadLine();

                if (!DateOnly.TryParseExact(dateConsole, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                {
                    Console.WriteLine("Invalid date format. Expected yyyy-MM-dd.");
                    return;
                }
                var dayOfWeek = date.DayOfWeek;

                Console.Write("Enter time (HH:mm): ");
                var timeConsole = Console.ReadLine();

                if (!TimeOnly.TryParseExact(timeConsole, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var time))
                {
                    Console.WriteLine("Invalid time format. Expected HH:mm.");
                    return;
                }
                RestrictionsConfig restrictionConfig = new RestrictionsConfig();
                Restrictions restriction = new Restrictions(restrictionConfig);
                Console.WriteLine(restriction.CheckRestriction(lastDigit, dayOfWeek, time).Reason);
                Console.WriteLine("===========================================");
                Console.WriteLine("\n");
            }

        }
    }
}