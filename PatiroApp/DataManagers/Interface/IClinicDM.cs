﻿using PatiroApp.Models;
using System.Collections.Generic;

namespace PatiroApp.DataManagers.Interface
{
    public interface IClinicDM
    {
        List<Clinic> GetClinics();

        void UpdateClinic(Clinic newClinic, User user);
    }
}
