using PatiroApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatiroApp.DataManagers
{
    public class Seed
    {
        public static void SeedData()
        {
            var user1 = new User { Username = "Admin", Roles = new List<string> { Role.Admin } };
            var user2 = new User { Username = "Employee", Roles = new List<string> { Role.Employee } };
            var user3 = new User { Username = "Partner", Roles = new List<string> { Role.Partner } };

            ClinicDM._clinics = new List<Clinic>
            {
                new Clinic { Id = 1, Name = "test", Description = "ttttttttt", IsActive = true, City = "aalborg", ZipCode = "1234", Members = null, CreatedAt = new System.DateTimeOffset(), CreatedBy = user1},
                new Clinic { Id = 2, Name = "test1", Description = "ttttttttt", IsActive = true, City = "aalborg", ZipCode = "9200", Members = new List<User>{ user3, user1}, CreatedAt = new System.DateTimeOffset(), CreatedBy = user2}
            };

            UserDM._users = new List<User>
            {
                user1,
                user2,
                user3
            };
        }
    }
}
