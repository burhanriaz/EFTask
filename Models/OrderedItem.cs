using System.Collections.Generic;

namespace EFTask.Models
{
    public class OrderedItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Sub_Total { get; set; }
      //  public decimal TotalPrice { get; set; }

        public Order Order { get; set; }
        public int OrderId_FK { get; set; }

        public UnitItem UnitItem { get; set; }
        public int UnitItemIdFK { get; set; }


    }
}
