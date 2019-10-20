using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatiroApp.DataManagers.Interface;
using PatiroApp.Models;

namespace PatiroApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserDM _userDM;
        public UsersController(IUserDM userDM)
        {
            _userDM = userDM;
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate(string username)
        {
            var user = _userDM.Authenticate(username);

            if (user == null)
                return BadRequest(new { message = "Username is incorrect" });

            return Ok(user);
        }

        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_userDM.GetUsers());
        }
    }
}
