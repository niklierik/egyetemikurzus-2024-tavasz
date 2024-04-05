using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell.Domain
{
    public sealed record class DagattMacska
    {
        public required string Name { get; init; }
    }
}
