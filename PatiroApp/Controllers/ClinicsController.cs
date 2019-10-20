using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatiroApp.DataManagers;
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
        public IActionResult Put(int id, [FromBody] Clinic newClinic)
        {
            var clinics = _clinicDM.GetClinics();
            User user = UserDM._users.Find(x => x.Username.Equals(User.Identity.Name));
            if (clinics.Exists(x => x.Id == id) && user != null)
            {
                newClinic.Id = id;
                _clinicDM.UpdateClinic(newClinic, user);
            }

            return Ok(clinics.Find(x => x.Id == id));
        }
    }
}