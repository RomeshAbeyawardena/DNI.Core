﻿using DNI.Shared.Services.Attributes;
using DNI.Shared.Shared.Attributes;
using System;

namespace DNI.Shared.App.Domains
{
    [MessagePack.MessagePackObject(true)]
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
