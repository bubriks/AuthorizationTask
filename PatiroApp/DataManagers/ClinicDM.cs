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
    }
}
