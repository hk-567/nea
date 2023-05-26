using System.Collections.Generic;
using System.Data;

namespace NEA.classes
{
    public class Service
    {
        public Dictionary<string, decimal[]> Rates { get; set; }
        public DataTable dt { get; set; }
        public string EmployeeType { get; set; }

        public Service()
        {
            Rates = new Dictionary<string, decimal[]>(); // services and their rates used for the invoice
            Rates.Add("Attorney Fee", new decimal[] { 8000m, 10000m }); // junior rate, senior rate
            Rates.Add("Documentary Stamp", new decimal[] { 1500m, 2000m });
            Rates.Add("Legal Counselling", new decimal[] { 4000m, 5000m });
            Rates.Add("Civil Litigation", new decimal[] { 75m, 93m });
            Rates.Add("Patent Application", new decimal[] { 12000m, 15000m });
            Rates.Add("Real Estate Title Search", new decimal[] { 2500m, 3500m });
            Rates.Add("Business Formation", new decimal[] { 5000m, 7500m });
            Rates.Add("Immigration Petition", new decimal[] { 6000m, 9000m });
            Rates.Add("Estate Planning", new decimal[] { 4500m, 6500m });
            Rates.Add("Trademark Registration", new decimal[] { 7000m, 9000m });
            Rates.Add("Employment Agreement", new decimal[] { 3000m, 4000m });
            Rates.Add("Arbitration", new decimal[] { 80m, 100m });
            Rates.Add("Mediation", new decimal[] { 100m, 120m });
        }

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
