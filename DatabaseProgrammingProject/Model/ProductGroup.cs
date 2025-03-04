using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ProductGroup
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        [ForeignKey(nameof(ParentID))]
        public int? ParentID { get; set; }

        public ProductGroup ParentGroup { get; set; }
        public ICollection<ProductGroup> ChildGroups { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
