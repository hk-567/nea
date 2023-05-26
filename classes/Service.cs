using System.Collections.Generic;
using System.Data;

namespace NEA.classes
{
    public class Service
    {
        // Define properties to store rates, DataTable, and employee type.
        public Dictionary<string, decimal>[] Rates { get; set; }
        public DataTable dt { get; set; }
        public string EmployeeType { get; set; }

        // Constructor to initialise rates and DataTable.
        public Service()
        {
            // Initialize rates for different legal services.
            Rates = new Dictionary<string, decimal[]>();
            Rates.Add("Attorney Fee", new decimal[] { 8000m, 10000m }); // junior rate, senior rate
            // more rates added here...
            Rates.Add("Mediation", new decimal[] { 100m, 120m });

            // Initialize DataTable with appropriate columns.
            dt = new DataTable();
            dt.Columns.Add(new DataColumn("No", typeof(int)));
            dt.Columns.Add(new DataColumn("Service", typeof(string)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("Hourly Rate", typeof(decimal)));
            dt.Columns.Add(new DataColumn("Quantity", typeof(int)));
            dt.Columns.Add(new DataColumn("Amount", typeof(decimal)));
        }
        // Gets the rate for a given service name and employee type
        public decimal GetRate(string serviceName)
        {
            decimal[] rates = Rates[serviceName];
            if (EmployeeType == "Junior")
            {
                return rates[0];
            }
            else
            {
                return rates[1];
            }
        }
    }
}

