using System.Collections.Generic;
using PatiroApp.DataManagers.Interface;
using PatiroApp.Models;

namespace PatiroApp.DataManagers
{
    public class ClinicDM : IClinicDM
    {
        public static List<Clinic> _clinics;

        public List<Clinic> GetClinics()
        {
            return _clinics;
        }

        public void UpdateClinic(Clinic newClinic, User user)
        {
            Clinic clinic = _clinics.Find(x => x.Id == newClinic.Id);
            if (user.Roles.Contains(Role.Admin) || user.Username.Equals(clinic.CreatedBy.Username))
            {
                clinic.Name = newClinic.Name;
                clinic.Description = newClinic.Description;
                clinic.IsActive = newClinic.IsActive;
                clinic.City = newClinic.City;
                clinic.ZipCode = newClinic.ZipCode;
                clinic.Members = newClinic.Members;
                clinic.CreatedAt = newClinic.CreatedAt;
                clinic.CreatedBy = newClinic.CreatedBy;
            }
            else if (user.Roles.Contains(Role.Employee) || user.Roles.Contains(Role.Partner))
            {
                if (user.Roles.Contains(Role.Employee))
                {
                    clinic.IsActive = newClinic.IsActive;
                }
                if (user.Roles.Contains(Role.Partner))
                {
                    if (clinic.Members.GetEnumerator() != null)
                        using (IEnumerator<User> empEnumerator = clinic.Members.GetEnumerator())
                        {
                            bool found = false;
                            while (empEnumerator.MoveNext() && !found)
                            {
                                if (empEnumerator.Current.Username.Equals(user.Username))
                                {
                                    clinic.Name = newClinic.Name;
                                    clinic.Description = newClinic.Description;
                                    clinic.City = newClinic.City;
                                    clinic.ZipCode = newClinic.ZipCode;
                                    if (newClinic.Members != null)
                                        clinic.Members = newClinic.Members;
                                    found = true;
                                }
                            }
                        }
                }
            }
        }
    }
}
