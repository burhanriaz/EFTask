using System.Collections.Generic;

namespace EFTask.Models
{
    public class OrderedItem
    {
        public Order Order { get; set; }
        public int?  OrderId_FK { get; set; }

        public Item Item { get; set; }
        public int? ItemId_Fk { get; set; }


        public Unit Unit { get; set; }
        public int?  UnitId_Fk { get; set; }


        public int Quantity { get; set; }
        public decimal Sub_Total { get; set; }

    }
}
