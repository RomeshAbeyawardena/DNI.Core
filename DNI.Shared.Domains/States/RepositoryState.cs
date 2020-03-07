
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Domains.States
{
    public class RepositoryState
    {
        public Type Type { get; set; }
        public EntityState State { get; set; }
        public object Value { get; set; }
    }
}
