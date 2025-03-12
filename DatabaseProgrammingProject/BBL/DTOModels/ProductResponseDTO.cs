using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBL.DTOModels
{
  
    public record ProductResponseDTO(int Id, string Name, decimal Price, string GroupName);

}
