using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PatiroApp.Models;
using Newtonsoft.Json;
using PatiroApp.DataManagers;
using System;
using System.Collections.Generic;

namespace PatiroApp
{
    public class ClinicAccessRequirement : IAuthorizationRequirement
    {
    }

    public class ClinicAccessHandler : AuthorizationHandler<ClinicAccessRequirement>
    {
        readonly IHttpContextAccessor _contextAccessor;

        public ClinicAccessHandler(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClinicAccessRequirement requirement)
        {
            var request = _contextAccessor.HttpContext.Request;

            //clinic info
            var newClinic = JsonConvert.DeserializeObject<Clinic>(OnAuthorizationAsync(request));
            var clinicID = Int32.Parse(_contextAccessor.HttpContext.Request.Query["id"]);
            var oldClinic = new ClinicDM().GetClinics().Find(x => x.Id == clinicID);

            newClinic.Id = oldClinic.Id;
            newClinic.CreatedBy = oldClinic.CreatedBy;

            //user info
            User user = UserDM._users.Find(x => x.Username.Equals(context.User.Identity.Name));

            if (user.Username.Equals(oldClinic.CreatedBy.Username) || user.Roles.Contains(Role.Admin))
            {
                _contextAccessor.HttpContext.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(newClinic)));

                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            else if (user.Roles.Contains(Role.Employee) || user.Roles.Contains(Role.Partner))
            {
                Clinic clinic = new Clinic { Id = oldClinic.Id, Name = oldClinic.Name, Description = oldClinic.Description, IsActive = oldClinic.IsActive, City = oldClinic.City, ZipCode = oldClinic.ZipCode, Members = oldClinic.Members, CreatedAt = oldClinic.CreatedAt, CreatedBy = oldClinic.CreatedBy };

                if (user.Roles.Contains(Role.Employee))
                {
                    clinic.IsActive = newClinic.IsActive;
                }
                if (user.Roles.Contains(Role.Partner))
                {
                    try
                    {
                        using (IEnumerator<User> empEnumerator = oldClinic.Members.GetEnumerator())
                        {
                            bool done = false;
                            while (empEnumerator.MoveNext() && !done)
                            {
                                if (empEnumerator.Current.Username.Equals(user.Username))
                                {
                                    clinic.Id = clinic.Id;
                                    clinic.Name = newClinic.Name;
                                    clinic.Description = newClinic.Description;
                                    clinic.City = newClinic.City;
                                    clinic.ZipCode = newClinic.ZipCode;
                                    if(newClinic.Members != null)
                                        clinic.Members = newClinic.Members;
                                    done = true;
                                }
                            }
                        }
                    }
                    catch (Exception e) { }
                }

                _contextAccessor.HttpContext.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(clinic)));
                
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            context.Fail();
            return Task.CompletedTask;
        }

        public string OnAuthorizationAsync(HttpRequest request)
        {
            var bodyStr = "";

            using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                bodyStr = reader.ReadToEndAsync().Result;
            }

            return bodyStr;
        }
    }
}
