using System;
using System.Collections.Generic;

namespace CinemaHub.DataAccess
{
    
    public class CustomerDL
    {
        // The list that holds all customer records in memory
        private List<string> customers;

        public CustomerDL()
        {
            customers = new List<string>();
        }

        // Returns the full list of customers
        public List<string> GetAllCustomers()
        {
            return customers;
        }

        // Adds one customer record to the list
        public void AddCustomer(string customerLine)
        {
            customers.Add(customerLine);
        }

        // Searches for a customer using their CNIC number
        public string SearchByCNIC(string cnic)
        {
            for (int i = 0; i < customers.Count; i++)
            {
                // Split the line to get each field
                string[] parts = customers[i].Split('|');

               
                if (parts[1] == cnic)
                {
                    return customers[i]; 
                }
            }

            return null; 
        }

        // Returns the total number of customers registered
        public int GetCustomerCount()
        {
            return customers.Count;
        }
    }
}