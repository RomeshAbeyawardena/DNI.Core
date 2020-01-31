using DNI.Shared.Services.Attributes;
using System;

namespace DNI.Shared.App.Domains
{
    public class UserSession
    {
        [Claim]
        public Guid SessionId { get; set; }
        
        [Claim]
        public int RoleId { get; set; }
        
        [Claim("Reference")]
        public string Username { get; set; }
    }
}
