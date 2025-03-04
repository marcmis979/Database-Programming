using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Product
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey(nameof(GroupID))]
        public int? GroupID { get; set; }


        public virtual ProductGroup? Group { get; set; }
        public ICollection<BasketPosition> BasketPositions { get; set; }
        public ICollection<OrderPosition> OrderPositions { get; set; }
    }
}
