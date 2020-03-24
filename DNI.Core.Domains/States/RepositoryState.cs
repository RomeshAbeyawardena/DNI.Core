namespace DNI.Core.Domains.States
{
    using System;
    using Microsoft.EntityFrameworkCore;

    public class RepositoryState
    {
        public Type Type { get; set; }

        public EntityState State { get; set; }

        public object Value { get; set; }
    }
}
