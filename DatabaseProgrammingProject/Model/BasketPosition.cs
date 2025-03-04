using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class BasketPosition
    {
        [Key, Column(Order = 0)]
        public int ProductID { get; set; }
        [Key, Column(Order = 1)]
        public int UserID { get; set; }
        public int Amount { get; set; }

        [ForeignKey(nameof(ProductID))]
        public virtual Product Product { get; set; }

        [ForeignKey(nameof(UserID))]
        public virtual User User { get; set; }
    }
}
