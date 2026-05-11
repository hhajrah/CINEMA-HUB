using System;
using CinemaHub.BusinessLogic;
using CinemaHub.DataAccess;
using CinemaHub.UI;

namespace CinemaHub
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] movies = { "Oppenheimer", "Ponyo", "Shawshank Redemption", "Arrival" };
            string[] showTimes = { "10:00 AM", "2:00 PM", "6:00 PM", "9:00 PM" };

            BookingBL bookingBL = new BookingBL();
            CustomerBL customerBL = new CustomerBL();
            AdminBL adminBL = new AdminBL();
            BookingDL bookingDL = new BookingDL();
            CustomerDL customerDL = new CustomerDL();
            AdminUI adminUI = new AdminUI();
            CustomerUI customerUI = new CustomerUI();

            
            bookingDL.LoadFromFileSilent();

            int choice = 0;
            while (choice != 3)
            {
                customerUI.ShowMainHeader();
                Console.WriteLine("  ========== MAIN MENU ==========\n");
                Console.WriteLine("  1. Admin Login");
                Console.WriteLine("  2. Customer Portal");
                Console.WriteLine("  3. Exit");
                Console.WriteLine("\n  --------------------------------");

                choice = customerUI.ReadInt("  Enter your choice: ");

                if (choice == 1)
                {
                    adminBL.AdminFlow(bookingBL, customerBL, bookingDL, customerDL, adminUI, movies, showTimes);
                }
                else if (choice == 2)
                {
                    customerBL.CustomerFlow(bookingBL, bookingDL, customerDL, customerUI, movies, showTimes);
                }
                else if (choice == 3)
                {
                    customerUI.ShowExitScreen();
                }
                else
                {
                    Console.WriteLine("\n  Invalid choice! Please enter 1, 2, or 3.");
                    customerUI.Pause();
                }
            }
        }
    }
}