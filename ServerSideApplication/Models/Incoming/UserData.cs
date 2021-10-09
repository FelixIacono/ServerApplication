using System.Collections.Generic;

namespace ServerSideApplication.Models.Incoming
{
    public class UserData // Incoming model - from POST request
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Interests { get; set; }
    }
}
