using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Entities
{
    public sealed class EX
    {
        // PK: FRAN, BRCH, WHSE, CORDTYPE, CORDNO
        public string Name { get; set; } = null!;
        public string Std { get; set; } = null!;
    }
}
