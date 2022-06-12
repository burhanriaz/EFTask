using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFTask.Models
{
    public class Item
    {

        public int? ItemId { get; set; }
        [Required(ErrorMessage ="Provide Name")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "Provide Price")]
        public decimal Price { get; set; } 
        public string Imgurl { get; set; }

        public  ICollection<UnitItem> UnitItems { get;  set; }=new HashSet<UnitItem>();
    }
}
