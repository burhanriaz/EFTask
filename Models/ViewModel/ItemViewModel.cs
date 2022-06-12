using System.Collections.Generic;

namespace EFTask.Models.ViewModel
{
    public class ItemViewModel
    {

        public IEnumerable<Item> Items { get; set; }
        public IEnumerable<Unit> Units { get; set; }
        public int? ItemId { get; set; }
        public int? UnitId { get; set; }
        public string UnitType { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Imgurl { get; set; }
    }
}
