using System;

namespace NEA.classes;
{
    // Define a public class ClientModel to encapsulate client data.
    public class ClientModel
    {
        // Define properties with get and set accessors to encapsulate data and allow for data validation.
        public string Name { get; set; }
        public string Gender { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNo { get; set; }
        public string HomeAddress { get; set; }
        public int UserId { get; set; }
    }
}

