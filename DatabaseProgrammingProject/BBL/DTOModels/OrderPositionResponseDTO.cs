using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBL.DTOModels
{
    public record OrderPositionResponseDTO(int OrderID, int ProductID, int Amount, double Price, string ProductName);
}
