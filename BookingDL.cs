using System;
using System.Collections.Generic;
using System.IO;

namespace CinemaHub.DataAccess
{
    public class BookingDL
    {
        private List<string> bookings;
        private int bookingIDCounter;
        private int ticketCounter;
        private string fileName;

        public BookingDL()
        {
            bookings = new List<string>();
            bookingIDCounter = 1000;
            ticketCounter = 1000;
            fileName = "bookings.txt";
        }

        public int GetTicketCounter()
        {
            return ticketCounter;
        }

        public void SetTicketCounter(int newValue)
        {
            ticketCounter = newValue;
        }

        public int GetNextBookingID()
        {
            bookingIDCounter = bookingIDCounter + 1;
            return bookingIDCounter;
        }

        public void AddBooking(string bookingLine)
        {
            bookings.Add(bookingLine);
        }

        public List<string> GetAllBookings()
        {
            return bookings;
        }

        public string SearchByTicketID(string ticketID)
        {
            for (int i = 0; i < bookings.Count; i++)
            {
                string[] parts = bookings[i].Split('|');
                string[] ids = parts[8].Split(',');

                for (int j = 0; j < ids.Length; j++)
                {
                    string cleanID = RemoveSpaces(ids[j]);
                    if (cleanID == ticketID)
                    {
                        return bookings[i];
                    }
                }
            }
            return null;
        }

        public bool DeleteSingleTicket(string ticketID)
        {
            for (int i = 0; i < bookings.Count; i++)
            {
                string[] parts = bookings[i].Split('|');
                string[] ids = parts[8].Split(',');

                bool ticketIsHere = false;
                for (int j = 0; j < ids.Length; j++)
                {
                    if (RemoveSpaces(ids[j]) == ticketID)
                    {
                        ticketIsHere = true;
                        break;
                    }
                }

                if (ticketIsHere == true)
                {
                    string newIDList = "";
                    int remainingTickets = 0;

                    for (int j = 0; j < ids.Length; j++)
                    {
                        string cleanID = RemoveSpaces(ids[j]);

                        if (cleanID != ticketID)
                        {
                            if (newIDList == "")
                                newIDList = cleanID;
                            else
                                newIDList = newIDList + "," + cleanID;

                            remainingTickets = remainingTickets + 1;
                        }
                    }

                    if (remainingTickets == 0)
                    {
                        bookings.RemoveAt(i);
                    }
                    else
                    {
                        int oldTicketCount = int.Parse(parts[6]);
                        int oldTotalPrice = int.Parse(parts[7]);
                        int pricePerTicket = oldTotalPrice / oldTicketCount;
                        int newTotalPrice = pricePerTicket * remainingTickets;

                        parts[6] = remainingTickets.ToString();
                        parts[7] = newTotalPrice.ToString();
                        parts[8] = newIDList;

                        string updatedLine = parts[0] + "|" + parts[1] + "|" + parts[2]
                            + "|" + parts[3] + "|" + parts[4] + "|" + parts[5]
                            + "|" + parts[6] + "|" + parts[7] + "|" + parts[8];

                        bookings[i] = updatedLine;
                    }

                    return true;
                }
            }
            return false;
        }

        public void SaveToFile()
        {
            StreamWriter writer = new StreamWriter(fileName);

            for (int i = 0; i < bookings.Count; i++)
            {
                writer.WriteLine(bookings[i]);
            }

            writer.Close();
            Console.WriteLine("\n  All bookings saved to file: " + fileName);
        }

       
        public void LoadFromFile()
        {
            LoadData(true);
        }

        
        public void LoadFromFileSilent()
        {
            LoadData(false);
        }

        private void LoadData(bool showMessages)
        {
            if (File.Exists(fileName) == false)
            {
                if (showMessages)
                    Console.WriteLine("\n  No saved file found. Nothing was loaded.");
                return;
            }

            bookings.Clear();

            StreamReader reader = new StreamReader(fileName);
            string line = reader.ReadLine();

            while (line != null)
            {
                if (line != "")
                {
                    bookings.Add(line);
                }
                line = reader.ReadLine();
            }

            reader.Close();

            if (showMessages)
                Console.WriteLine("\n  Bookings loaded successfully! Total: " + bookings.Count);
        }

        private string RemoveSpaces(string text)
        {
            string result = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != ' ')
                {
                    result = result + text[i];
                }
            }
            return result;
        }
    }
}