﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBL.DTOModels
{
    public record OrderResponseDTO(int ID, double TotalValue, bool IsPaid, DateTime Date);
}
