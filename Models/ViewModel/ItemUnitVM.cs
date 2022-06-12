using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace EFTask.Models.ViewModel
{
    public class ItemUnitVM
    {
       public Item Item { get; set; }
        public IList<SelectListItem> Unit { get; set; }
        public UnitItem UnitItems { get; set; }  

       // public IEnumerable<SelectListItem> UnitList { get; set; }

    }
}
