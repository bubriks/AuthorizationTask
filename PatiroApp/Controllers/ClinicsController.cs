using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatiroApp.DataManagers.Interface;
using PatiroApp.Models;

namespace PatiroApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ClinicsController : ControllerBase
    {
        private readonly IClinicDM _clinicDM;
        public ClinicsController(IClinicDM clinicDM)
        {
            _clinicDM = clinicDM;
        }

        [Authorize(Roles = "" + Role.Admin + ", " +Role.Employee)]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_clinicDM.GetClinics());
        }

        [HttpPut]
        [Authorize(Policy = "ClinicModifyAccess")]
        public IActionResult Put(int id, [FromBody] Clinic newClinic)
        {
            var clinics = _clinicDM.GetClinics();
            if (clinics.Exists(x => x.Id == id))
            {
                var index = clinics.FindIndex(x => x.Id == id);
                clinics[index] = newClinic;
            }
            else
            {
                newClinic.Id = id;
                clinics.Add(newClinic);
            }

            return Ok(clinics.Find(x => x.Id == id));
        }
    }
}