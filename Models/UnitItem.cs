using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFTask.Models
{
    public class UnitItem
    {
     
        [Key]
       public int UnitItemId { get; set; }
        public Item  Item { get; set; }
       
        public int ItemId { get; set; }
        public  Unit Unit { get; set; }
   
        public int UnitId { get; set; }

      public ICollection<OrderedItem> OrderItem { get; set; }

    }
}
