using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PatiroApp.DataManagers.Interface;
using PatiroApp.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace PatiroApp.DataManagers
{
    public class UserDM: IUserDM
    {
        public static List<User> _users;
        private readonly AppSettings _appSettings;

        public UserDM(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public User Authenticate(string username)
        {
            var user = _users.SingleOrDefault(x => x.Username == username);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.Username));
            foreach (String r in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, r));
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user;
        }

        public List<User> GetUsers()
        {
            return _users;
        }
    }
}
