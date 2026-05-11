using System;

namespace CinemaHub.UI
{
    public class AdminUI
    {
        private string adminTitle;

        public AdminUI()
        {
            adminTitle = "ADMIN PANEL";
        }

        public string GetAdminTitle()
        {
            return adminTitle;
        }

        private void ClearScreen()
        {
            try
            {
                Console.Clear();
            }
            catch
            {
                for (int i = 0; i < 50; i++)
                {
                    Console.WriteLine();
                }
            }
        }

        public void ShowAdminMenu()
        {
            ClearScreen();
            Console.WriteLine("\n  ========== " + adminTitle + " ==========\n");
            Console.WriteLine("  1. View All Bookings");
            Console.WriteLine("  2. Search Booking by Ticket ID");
            Console.WriteLine("  3. Delete Single Ticket");
            Console.WriteLine("  4. View Earnings Report");
            Console.WriteLine("  5. Logout");
            Console.WriteLine("  6. Save All Data to File");
            Console.WriteLine("  7. Load All Data from File");
            Console.WriteLine("\n  --------------------------------");
        }

        public void ShowLoginScreen()
        {
            ClearScreen();
            Console.WriteLine("\n======================================================");
            Console.WriteLine("  ADMIN LOGIN");
            Console.WriteLine("======================================================\n");
        }

        public void ShowHeader(string title)
        {
            ClearScreen();
            Console.WriteLine("\n======================================================");
            Console.WriteLine("  " + title);
            Console.WriteLine("======================================================\n");
        }

        public void DisplayBooking(string bookingLine)
        {
            string[] parts = bookingLine.Split('|');

            Console.WriteLine("  ----------------------------------------------------");
            Console.WriteLine("  Booking ID    : " + parts[0]);
            Console.WriteLine("  Customer      : " + parts[1]);
            Console.WriteLine("  CNIC          : " + parts[2]);
            Console.WriteLine("  Movie         : " + parts[3]);
            Console.WriteLine("  Show Time     : " + parts[4]);
            Console.WriteLine("  Seat Type     : " + parts[5]);
            Console.WriteLine("  Tickets       : " + parts[6]);
            Console.WriteLine("  Total Price   : Rs " + parts[7]);
            Console.WriteLine("  Ticket IDs    : " + parts[8]);
            Console.WriteLine("  ----------------------------------------------------");
        }

        public int ReadInt(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();

            int result;
            bool isValid = ParseInt(input, out result);

            while (isValid == false)
            {
                Console.Write("  Please enter a valid number: ");
                input = Console.ReadLine();
                isValid = ParseInt(input, out result);
            }

            return result;
        }

        public string ReadLine(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();

            if (input == null)
            {
                input = "";
            }

            return input;
        }

        public void Pause()
        {
            Console.Write("\n  Press any key to continue...");
            Console.ReadKey();
        }

        private bool ParseInt(string input, out int result)
        {
            result = 0;

            if (input == null || input == "") return false;

            int startIndex = 0;
            int sign = 1;

            if (input[0] == '-')
            {
                sign = -1;
                startIndex = 1;
            }
            else if (input[0] == '+')
            {
                startIndex = 1;
            }

            if (startIndex == input.Length) return false;

            for (int i = startIndex; i < input.Length; i++)
            {
                char c = input[i];

                if (c < '0' || c > '9')
                {
                    return false;
                }

                int digitValue = c - '0';
                result = result * 10 + digitValue;
            }

            result = result * sign;

            return true;
        }
    }
}