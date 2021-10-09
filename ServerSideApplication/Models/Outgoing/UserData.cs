using System;
using System.Collections.Generic;

namespace ServerSideApplication.Models.Outgoing
{
    public class UserData // Outgoing - served via GET request
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Interests { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public string Device { get; set; }
        public string IP { get; set; }
        public string Location { get; set; }
    }
}
