using System;
using System.Collections.Generic;
using CinemaHub.DataAccess;
using CinemaHub.UI;

namespace CinemaHub.BusinessLogic
{
    public class CustomerBL
    {
        private string adminUsername;
        private string adminPassword;
        private string currentCustomerCNIC;

        public CustomerBL()
        {
            adminUsername = "Hajra";
            adminPassword = "admin@321";
            currentCustomerCNIC = "";
        }

        public string GetAdminUsername() { return adminUsername; }
        public string GetAdminPassword() { return adminPassword; }

        public bool IsValidAdminLogin(string username, string password)
        {
            if (username == adminUsername && password == adminPassword)
                return true;
            return false;
        }

        public bool IsValidName(string name)
        {
            if (name == null || name == "") return false;

            for (int i = 0; i < name.Length; i++)
            {
                char letter = name[i];
                bool isLowerCase = (letter >= 'a' && letter <= 'z');
                bool isUpperCase = (letter >= 'A' && letter <= 'Z');
                bool isSpace = (letter == ' ');

                if (isLowerCase == false && isUpperCase == false && isSpace == false)
                    return false;
            }
            return true;
        }

        public bool IsValidCNIC(string cnic)
        {
            if (cnic == null || cnic == "") return false;
            if (cnic.Length != 13) return false;

            for (int i = 0; i < cnic.Length; i++)
            {
                char digit = cnic[i];
                if (digit < '0' || digit > '9') return false;
            }
            return true;
        }

        public bool IsValidPhone(string phone)
        {
            if (phone == null || phone == "") return false;
            if (phone.Length != 11) return false;

            for (int i = 0; i < phone.Length; i++)
            {
                char digit = phone[i];
                if (digit < '0' || digit > '9') return false;
            }
            return true;
        }

        public void CustomerFlow(BookingBL bookingBL, BookingDL bookingDL,
            CustomerDL customerDL, CustomerUI customerUI,
            string[] movies, string[] showTimes)
        {
            string name = RegisterCustomer(customerDL, customerUI);

            if (name != "")
            {
                ShowCustomerMenu(name, bookingBL, bookingDL, customerDL, customerUI, movies, showTimes);
            }
        }

        private string RegisterCustomer(CustomerDL customerDL, CustomerUI customerUI)
        {
            customerUI.ShowRegistrationScreen();

            string name = "";
            bool gotName = false;
            while (gotName == false)
            {
                name = customerUI.ReadLine("  Full Name : ");
                if (IsValidName(name) == true)
                    gotName = true;
                else
                    Console.WriteLine("  Invalid! Use letters and spaces only.\n");
            }

            string cnic = "";
            bool gotCNIC = false;
            while (gotCNIC == false)
            {
                cnic = customerUI.ReadLine("  CNIC (13 digits, no dashes) : ");
                if (IsValidCNIC(cnic) == true)
                    gotCNIC = true;
                else
                    Console.WriteLine("  Invalid! Must be exactly 13 digits.\n");
            }

            string phone = "";
            bool gotPhone = false;
            while (gotPhone == false)
            {
                phone = customerUI.ReadLine("  Phone (11 digits) : ");
                if (IsValidPhone(phone) == true)
                    gotPhone = true;
                else
                    Console.WriteLine("  Invalid! Must be exactly 11 digits.\n");
            }

            string customerRecord = name + "|" + cnic + "|" + phone;
            customerDL.AddCustomer(customerRecord);

            currentCustomerCNIC = cnic;

            Console.WriteLine("\n  Registration successful! Welcome, " + name + "!");
            customerUI.Pause();

            return name;
        }

        private void ShowCustomerMenu(string customerName, BookingBL bookingBL,
            BookingDL bookingDL, CustomerDL customerDL, CustomerUI customerUI,
            string[] movies, string[] showTimes)
        {
            int choice = 0;
            while (choice != 4)
            {
                customerUI.ShowCustomerMenu(customerName);
                choice = customerUI.ReadInt("  Enter choice: ");

                if (choice == 1)
                    ShowMovieList(movies, customerUI);
                else if (choice == 2)
                    BookTicket(customerName, bookingBL, bookingDL, customerUI, movies, showTimes);
                else if (choice == 3)
                    ViewMyBookings(customerName, bookingBL, bookingDL, customerUI);
                else if (choice == 4)
                    Console.WriteLine("\n  Logging out...");
                else
                {
                    Console.WriteLine("\n  Invalid choice! Please enter 1 to 4.");
                    customerUI.Pause();
                }
            }
        }

        private void ShowMovieList(string[] movies, CustomerUI customerUI)
        {
            customerUI.ShowHeader("NOW SHOWING");
            Console.WriteLine("  No.   Movie Title");
            Console.WriteLine("  ----------------------------------------");

            for (int i = 0; i < movies.Length; i++)
            {
                Console.WriteLine("  " + (i + 1) + ".    " + movies[i]);
            }

            Console.WriteLine("  ----------------------------------------");
            customerUI.Pause();
        }

        private void BookTicket(string customerName, BookingBL bookingBL,
            BookingDL bookingDL, CustomerUI customerUI,
            string[] movies, string[] showTimes)
        {
            customerUI.ShowHeader("BOOK TICKET");
            Console.WriteLine("  Customer: " + customerName + "\n");

            //  Step 1: Pick a movie
         
            int movieChoice = 0;
            while (movieChoice < 1 || movieChoice > movies.Length)
            {
                Console.WriteLine("  Select Movie:\n");
                for (int i = 0; i < movies.Length; i++)
                {
                    Console.WriteLine("  " + (i + 1) + ". " + movies[i]);
                }
                movieChoice = customerUI.ReadInt("\n  Enter choice (1-" + movies.Length + "): ");
            }

            string selectedMovie = movies[movieChoice - 1];

            //  Step 2: Pick a show time 
            
            int timeChoice = 0;
            while (timeChoice < 1 || timeChoice > showTimes.Length)
            {
                Console.WriteLine("\n  Select Show Time:\n");
                for (int i = 0; i < showTimes.Length; i++)
                {
                    Console.WriteLine("  " + (i + 1) + ". " + showTimes[i]);
                }
                timeChoice = customerUI.ReadInt("\n  Enter choice (1-" + showTimes.Length + "): ");
            }

            string selectedTime = showTimes[timeChoice - 1];

            // Step 3: How many tickets?
            int numTickets = 0;
            while (bookingBL.IsValidTicketCount(numTickets) == false)
            {
                numTickets = customerUI.ReadInt("  Number of Tickets (1-20): ");
                if (bookingBL.IsValidTicketCount(numTickets) == false)
                    Console.WriteLine("  Invalid! Please enter a number between 1 and 20.");
            }

            //  Step 4: Choose seat type 
            Console.WriteLine("\n  Select Seat Type:\n");
            Console.WriteLine("  1. Standard  - Rs 600 per ticket");
            Console.WriteLine("  2. Gold      - Rs 900 per ticket");
            Console.WriteLine("  3. Platinum  - Rs 1500 per ticket");

            int seatChoice = 0;
            while (seatChoice < 1 || seatChoice > 3)
            {
                seatChoice = customerUI.ReadInt("\n  Enter choice (1-3): ");
            }

            string seatType = "";
            if (seatChoice == 1) seatType = "Standard";
            else if (seatChoice == 2) seatType = "Gold";
            else seatType = "Platinum";

            //  Step 5: Calculate price and generate IDs 
            int totalPrice = bookingBL.CalculateTotalPrice(seatType, numTickets);

            int counter = bookingDL.GetTicketCounter();
            int newCounter;
            List<string> ticketIDs = bookingBL.GenerateTicketIDs(counter, numTickets, out newCounter);
            bookingDL.SetTicketCounter(newCounter);

            string allIDs = "";
            for (int i = 0; i < ticketIDs.Count; i++)
            {
                if (i == 0) allIDs = ticketIDs[i];
                else allIDs = allIDs + "," + ticketIDs[i];
            }

            int bookingID = bookingDL.GetNextBookingID();

            //  Show summary 
            Console.WriteLine("\n  --------------------------------------------------");
            Console.WriteLine("  Movie         : " + selectedMovie);
            Console.WriteLine("  Show Time     : " + selectedTime);
            Console.WriteLine("  Seat Type     : " + seatType);
            Console.WriteLine("  Tickets       : " + numTickets);
            Console.WriteLine("  Price/ticket  : Rs " + bookingBL.GetPricePerTicket(seatType));
            Console.WriteLine("  Total Bill    : Rs " + totalPrice);
            Console.WriteLine("  Ticket IDs    : " + allIDs);
            Console.WriteLine("  --------------------------------------------------");

            // Step 7: Confirm 
            string confirm = customerUI.ReadLine("\n  Confirm booking? (Y/N): ");

            if (confirm != "Y" && confirm != "y")
            {
                Console.WriteLine("\n  Booking cancelled.");
                customerUI.Pause();
                return;
            }

            string bookingRecord = bookingID + "|" + customerName + "|" + currentCustomerCNIC + "|"
                + selectedMovie + "|" + selectedTime + "|" + seatType + "|"
                + numTickets + "|" + totalPrice + "|" + allIDs;

            bookingDL.AddBooking(bookingRecord);
            Console.WriteLine("\n  Booking confirmed! Your Booking ID: " + bookingID);
            customerUI.Pause();
        }

        private void ViewMyBookings(string customerName, BookingBL bookingBL,
            BookingDL bookingDL, CustomerUI customerUI)
        {
            customerUI.ShowHeader("MY BOOKINGS");

            List<string> allBookings = bookingDL.GetAllBookings();
            List<string> myBookings = bookingBL.GetBookingsByCustomer(allBookings, customerName);

            if (myBookings.Count == 0)
            {
                Console.WriteLine("  You have no bookings yet.");
            }
            else
            {
                for (int i = 0; i < myBookings.Count; i++)
                {
                    Console.WriteLine("\n  MY BOOKING #" + (i + 1));
                    customerUI.DisplayBooking(myBookings[i]);
                }
                Console.WriteLine("\n  Total: " + myBookings.Count + " booking(s).");
            }

            customerUI.Pause();
        }
    }
}