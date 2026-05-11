using System;
using System.Collections.Generic;
using CinemaHub.DataAccess;
using CinemaHub.UI;

namespace CinemaHub.BusinessLogic
{
    public class AdminBL
    {
        public void AdminFlow(BookingBL bookingBL, CustomerBL customerBL,
            BookingDL bookingDL, CustomerDL customerDL, AdminUI adminUI,
            string[] movies, string[] showTimes)
        {
            bool loginSuccess = TryAdminLogin(customerBL, adminUI);

            
            if (loginSuccess == false) return;

            ShowAdminMenu(bookingBL, customerBL, bookingDL, customerDL, adminUI, movies, showTimes);
        }

       
        private bool TryAdminLogin(CustomerBL customerBL, AdminUI adminUI)
        {
            adminUI.ShowLoginScreen();

            int attemptsUsed = 0;

            while (attemptsUsed < 3)
            {
                string username = adminUI.ReadLine("  Username : ");
                string password = adminUI.ReadLine("  Password : ");

                if (customerBL.IsValidAdminLogin(username, password) == true)
                {
                    Console.WriteLine("\n  Login successful! Welcome, Admin!");
                    adminUI.Pause();
                    return true; 
                }

                attemptsUsed = attemptsUsed + 1;
                int attemptsLeft = 3 - attemptsUsed;
                Console.WriteLine("  Wrong credentials! Attempts remaining: " + attemptsLeft + "\n");
            }

            
            Console.WriteLine("\n  Access denied. Too many failed attempts.");
            adminUI.Pause();
            return false;
        }

     
        private void ShowAdminMenu(BookingBL bookingBL, CustomerBL customerBL,
            BookingDL bookingDL, CustomerDL customerDL, AdminUI adminUI,
            string[] movies, string[] showTimes)
        {
            int choice = 0;

            while (choice != 5)
            {
                adminUI.ShowAdminMenu();
                choice = adminUI.ReadInt("  Enter your choice: ");

                if (choice == 1)
                {
                    ViewAllBookings(bookingDL, adminUI);
                }
                else if (choice == 2)
                {
                    SearchBookingByTicketID(bookingDL, adminUI);
                }
                else if (choice == 3)
                {
                    DeleteTicket(bookingDL, adminUI);
                }
                else if (choice == 4)
                {
                    ViewEarningsReport(bookingBL, bookingDL, customerDL, adminUI);
                }
                else if (choice == 5)
                {
                    Console.WriteLine("\n  Logging out...");
                }
                else if (choice == 6)
                {
                    bookingDL.SaveToFile();
                    adminUI.Pause();
                }
                else if (choice == 7)
                {
                    bookingDL.LoadFromFile();
                    adminUI.Pause();
                }
                else
                {
                    Console.WriteLine("\n  Invalid choice! Please enter 1 to 7.");
                    adminUI.Pause();
                }
            }
        }

       
        public void AddNewBooking(BookingBL bookingBL, CustomerBL customerBL,
            BookingDL bookingDL, CustomerDL customerDL, AdminUI adminUI,
            string[] movies, string[] showTimes)
        {
            adminUI.ShowHeader("ADD NEW BOOKING");

            //name
            string name = "";
            bool gotName = false;

            while (gotName == false)
            {
                name = adminUI.ReadLine("  Customer Name : ");

                if (customerBL.IsValidName(name) == true)
                {
                    gotName = true;
                }
                else
                {
                    Console.WriteLine("  Invalid! Use letters and spaces only.\n");
                }
            }

            //cnic
            string cnic = "";
            bool gotCNIC = false;

            while (gotCNIC == false)
            {
                cnic = adminUI.ReadLine("  CNIC (13 digits) : ");

                if (customerBL.IsValidCNIC(cnic) == true)
                {
                    gotCNIC = true;
                }
                else
                {
                    Console.WriteLine("  Invalid! Must be exactly 13 digits.\n");
                }
            }

            //pick a movie
            Console.WriteLine("\n  Select Movie:\n");
            for (int i = 0; i < movies.Length; i++)
            {
                Console.WriteLine("  " + (i + 1) + ". " + movies[i]);
            }

            int movieChoice = 0;
            while (movieChoice < 1 || movieChoice > movies.Length)
            {
                movieChoice = adminUI.ReadInt("\n  Enter choice (1-" + movies.Length + "): ");
            }

            string selectedMovie = movies[movieChoice - 1];

            //pick a showtime
            Console.WriteLine("\n  Select Show Time:\n");
            for (int i = 0; i < showTimes.Length; i++)
            {
                Console.WriteLine("  " + (i + 1) + ". " + showTimes[i]);
            }

            int timeChoice = 0;
            while (timeChoice < 1 || timeChoice > showTimes.Length)
            {
                timeChoice = adminUI.ReadInt("\n  Enter choice (1-" + showTimes.Length + "): ");
            }

            string selectedTime = showTimes[timeChoice - 1];

           //number of tickets
            int numTickets = 0;
            while (bookingBL.IsValidTicketCount(numTickets) == false)
            {
                numTickets = adminUI.ReadInt("  Number of Tickets (1-20): ");

                if (bookingBL.IsValidTicketCount(numTickets) == false)
                {
                    Console.WriteLine("  Invalid! Please enter a number between 1 and 20.");
                }
            }

            //seat type
            Console.WriteLine("\n  Select Seat Type:\n");
            Console.WriteLine("  1. Standard  - Rs 600 per ticket");
            Console.WriteLine("  2. Gold      - Rs 900 per ticket");
            Console.WriteLine("  3. Platinum  - Rs 1500 per ticket");

            int seatChoice = 0;
            while (seatChoice < 1 || seatChoice > 3)
            {
                seatChoice = adminUI.ReadInt("\n  Enter choice (1-3): ");
            }

            string seatType = "";
            if (seatChoice == 1) seatType = "Standard";
            else if (seatChoice == 2) seatType = "Gold";
            else seatType = "Platinum";

            //  Calculate price and generate ticket IDs 
            int totalPrice = bookingBL.CalculateTotalPrice(seatType, numTickets);

            int counter = bookingDL.GetTicketCounter();
            int newCounter;
            List<string> ticketIDs = bookingBL.GenerateTicketIDs(counter, numTickets, out newCounter);
            bookingDL.SetTicketCounter(newCounter);

            // Join IDs into one string: "TKT1001,TKT1002"
            string allIDs = "";
            for (int i = 0; i < ticketIDs.Count; i++)
            {
                if (i == 0) allIDs = ticketIDs[i];
                else allIDs = allIDs + "," + ticketIDs[i];
            }

            int bookingID = bookingDL.GetNextBookingID();

            
            Console.WriteLine("\n  --------------------------------------------------");
            Console.WriteLine("  Movie         : " + selectedMovie);
            Console.WriteLine("  Show Time     : " + selectedTime);
            Console.WriteLine("  Seat Type     : " + seatType);
            Console.WriteLine("  Tickets       : " + numTickets);
            Console.WriteLine("  Price/ticket  : Rs " + bookingBL.GetPricePerTicket(seatType));
            Console.WriteLine("  Total Bill    : Rs " + totalPrice);
            Console.WriteLine("  Ticket IDs    : " + allIDs);
            Console.WriteLine("  --------------------------------------------------");

          
            string confirm = adminUI.ReadLine("\n  Confirm booking? (Y/N): ");

            if (confirm != "Y" && confirm != "y")
            {
                Console.WriteLine("\n  Booking cancelled.");
                adminUI.Pause();
                return;
            }

            // Save booking record
            
            string bookingRecord = bookingID + "|" + name + "|" + cnic + "|"
                + selectedMovie + "|" + selectedTime + "|" + seatType + "|"
                + numTickets + "|" + totalPrice + "|" + allIDs;

            bookingDL.AddBooking(bookingRecord);

            // Also save a basic customer record
            string customerRecord = name + "|" + cnic;
            customerDL.AddCustomer(customerRecord);

            Console.WriteLine("\n  Booking confirmed! Booking ID: " + bookingID);
            adminUI.Pause();
        }

        // Shows every booking stored in the system
        private void ViewAllBookings(BookingDL bookingDL, AdminUI adminUI)
        {
            adminUI.ShowHeader("ALL BOOKINGS");

            List<string> allBookings = bookingDL.GetAllBookings();

            if (allBookings.Count == 0)
            {
                Console.WriteLine("  No bookings found in the system.");
            }
            else
            {
                for (int i = 0; i < allBookings.Count; i++)
                {
                    Console.WriteLine("\n  BOOKING #" + (i + 1));
                    adminUI.DisplayBooking(allBookings[i]);
                }

                Console.WriteLine("\n  Total bookings in system: " + allBookings.Count);
            }

            adminUI.Pause();
        }

        // Searches for a booking using a ticket ID like "TKT1001"
        private void SearchBookingByTicketID(BookingDL bookingDL, AdminUI adminUI)
        {
            adminUI.ShowHeader("SEARCH BOOKING");

            string ticketID = adminUI.ReadLine("  Enter Ticket ID (e.g. TKT1001): ");
            string foundRecord = bookingDL.SearchByTicketID(ticketID);

            if (foundRecord == null)
            {
                Console.WriteLine("\n  No booking found with Ticket ID: " + ticketID);
            }
            else
            {
                Console.WriteLine("\n  Booking found:");
                adminUI.DisplayBooking(foundRecord);
            }

            adminUI.Pause();
        }

        
        private void DeleteTicket(BookingDL bookingDL, AdminUI adminUI)
        {
            adminUI.ShowHeader("DELETE TICKET");

            List<string> allBookings = bookingDL.GetAllBookings();

            if (allBookings.Count == 0)
            {
                Console.WriteLine("  There are no bookings to delete.");
                adminUI.Pause();
                return;
            }

            string ticketID = adminUI.ReadLine("  Enter Ticket ID to delete (e.g. TKT1001): ");
            string foundRecord = bookingDL.SearchByTicketID(ticketID);

            if (foundRecord == null)
            {
                Console.WriteLine("\n  Ticket ID not found in the system.");
            }
            else
            {
                Console.WriteLine("\n  Booking found:");
                adminUI.DisplayBooking(foundRecord);

                string[] parts = foundRecord.Split('|');
                Console.WriteLine("\n  This booking contains " + parts[6] + " ticket(s).");
                Console.WriteLine("  Only ticket " + ticketID + " will be removed.\n");

                string confirm = adminUI.ReadLine("  Confirm deletion? (Y/N): ");

                if (confirm == "Y" || confirm == "y")
                {
                    bookingDL.DeleteSingleTicket(ticketID);
                    Console.WriteLine("\n  Ticket " + ticketID + " has been deleted.");
                }
                else
                {
                    Console.WriteLine("\n  Deletion cancelled.");
                }
            }

            adminUI.Pause();
        }

        // Shows a summary of total earnings and customer count
        private void ViewEarningsReport(BookingBL bookingBL, BookingDL bookingDL,
            CustomerDL customerDL, AdminUI adminUI)
        {
            adminUI.ShowHeader("EARNINGS REPORT");

            List<string> allBookings = bookingDL.GetAllBookings();
            int totalEarned = bookingBL.CalculateTotalEarnings(allBookings);

            Console.WriteLine("  Total Bookings  : " + allBookings.Count);
            Console.WriteLine("  Total Earnings  : Rs " + totalEarned);
            Console.WriteLine("  Total Customers : " + customerDL.GetCustomerCount());
            Console.WriteLine("\n  ==========================================");

            adminUI.Pause();
        }
    }
}