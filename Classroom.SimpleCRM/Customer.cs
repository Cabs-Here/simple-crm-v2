using System;
using System.Collections.Generic;
using System.Text;

namespace Classroom.SimpleCRM
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public InteractionMethod PreferredContactMethod { get; set; }
        public CustomerStatus Status { get; set; }
        public DateTime LastContactDate { get; set; }
    }
}
