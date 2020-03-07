
using Microsoft.EntityFrameworkCore;
using System;

namespace DNI.Shared.Domains.States
{
    public class RepositoryState
    {
        public Type Type { get; set; }
        public EntityState State { get; set; }
        public object Value { get; set; }
    }
}
