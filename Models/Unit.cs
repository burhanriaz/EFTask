using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFTask.Models
{
    public class Unit
    {
        public int UnitId { get; set; }
        [Required(ErrorMessage = "Provide Unit Type Like Litter or Kg")]
        [Display(Name = "Unit Type")]

        [StringLength(50, MinimumLength = 2)]
        public string UnitType { get; set; }
        // public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<UnitItem> UnitItems { get;  set; } = new List<UnitItem>();
        public virtual ICollection<OrderedItem> OrderedItems { get; set; } = new List<OrderedItem>();


    }
}
