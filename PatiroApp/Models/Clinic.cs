using System;
using System.Collections.Generic;

namespace PatiroApp.Models
{
    public class Clinic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public IEnumerable<User> Members { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public User CreatedBy { get; set; }
    }
}
