using System.Collections.Generic;

namespace bodyspace_api.Models
{
    public class LoginResult
    {
        public int UserId { get; set;}
        public string Token { get; set; }
        public IEnumerable<KeyValuePair<string, string>> ResponseHeaders { get; set; }
    }
}