using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PatiroApp.Models
{
    public class User
    {
        [Required]
        public string Username { get; set; }
        public List<string> Roles { get; set; }
        public string Token { get; set; }
    }
}
