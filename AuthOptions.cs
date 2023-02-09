using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Authenticate
{
    public class AuthOptions
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string Secret { get; set; }

        public int TokenLifetime { get; set; }

        public SymmetricSecurityKey GetSymmetricSecuritykey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));
        }
    }
}


