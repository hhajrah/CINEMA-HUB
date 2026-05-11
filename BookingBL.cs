using System;
using System.Collections.Generic;

namespace CinemaHub.BusinessLogic
{
    public class BookingBL
    {
       
        private int minTickets;
        private int maxTickets;

        
        public BookingBL()
        {
            minTickets = 1;
            maxTickets = 20;
        }

       
        public int GetMinTickets()
        {
            return minTickets;
        }

      
        public int GetMaxTickets()
        {
            return maxTickets;
        }

       
        public bool IsValidTicketCount(int count)
        {
            if (count >= minTickets && count <= maxTickets)
            {
                return true;   
            }
            return false;      
        }

        
        public int GetPricePerTicket(string seatType)
        {
            if (seatType == "Standard")
            {
                return 600;
            }
            else if (seatType == "Gold")
            {
                return 900;
            }
            else if (seatType == "Platinum")
            {
                return 1500;
            }

            
            return 0;
        }

       
        public int CalculateTotalPrice(string seatType, int numberOfTickets)
        {
            int priceForOne = GetPricePerTicket(seatType);
            int totalBill = priceForOne * numberOfTickets;
            return totalBill;
        }

       
        public List<string> GenerateTicketIDs(int startCounter, int count, out int updatedCounter)
        {
            
            List<string> ticketIDs = new List<string>();

            
            for (int i = 0; i < count; i++)
            {
                startCounter = startCounter + 1;           
                string newID = "TKT" + startCounter;       
                ticketIDs.Add(newID);                     
            }

           
            updatedCounter = startCounter;

            return ticketIDs;
        }

       
        public int CalculateTotalEarnings(List<string> allBookings)
        {
            int grandTotal = 0;

            for (int i = 0; i < allBookings.Count; i++)
            {
                
                string[] parts = allBookings[i].Split('|');

               
                int bookingPrice = int.Parse(parts[7]);

                grandTotal = grandTotal + bookingPrice;
            }

            return grandTotal;
        }

      
        public List<string> GetBookingsByCustomer(List<string> allBookings, string customerName)
        {
            List<string> myBookings = new List<string>();

            for (int i = 0; i < allBookings.Count; i++)
            {
                string[] parts = allBookings[i].Split('|');

                
                if (parts[1] == customerName)
                {
                    myBookings.Add(allBookings[i]);
                }
            }

            return myBookings;
        }
    }
}