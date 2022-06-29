using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFTask.Models
{
    public class UnitItem
    {
        // by using Virtual key word enable lazy loading 
        public virtual Item Item { get; set; }
        public int ItemId { get; set; }

        public virtual Unit Unit { get; set; }
        public int UnitId { get; set; }

    }
}
