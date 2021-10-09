using System;
using System.Collections.Generic;

namespace Domain
{
    public class UserData // Domain Object - Saved to / read from CSV
    {
        public Guid Id { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public string Device { get; set; }
        public string IP { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Interests { get; set; }
        public string Location { get; set; }
    }
}
