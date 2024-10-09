using Core.Entities.Concrete;
using System;
using System.Collections.Generic;

namespace Core.Utilities.Security.Jwt
{
    public class AccessToken : IAccessToken
    {
        public List<string> Claims { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string RefreshToken { get; set; }

        public List<Group> Groups { get; set; }
        public User User { get; set; }
    }
}