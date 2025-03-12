using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBL.DTOModels
{
    public record ProductGroupDTO(int ID, string Name, bool HasSubGroups);
}
