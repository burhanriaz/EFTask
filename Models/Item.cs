using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFTask.Models
{
    public class Item
    {

        public int? ItemId { get; set; }
        [Required(ErrorMessage ="Provide Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Provide Price")]
        public decimal Price { get; set; } 

        public virtual ICollection<UnitItem> UnitItems { get;  set; }=new HashSet<UnitItem>();
        public virtual ICollection<OrderedItem> OrderedItems { get; set; } = new List<OrderedItem>();

    }
}
