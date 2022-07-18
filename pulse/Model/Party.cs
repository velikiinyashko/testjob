using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pulse.Model
{
    public class Party
    {
        public int? PartyId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int RetailId { get; set; }
        public string? RetailName { get; set; }
        public int StockId { get; set; }
        public string? StockName { get; set; }
        public int Count { get; set; }
        public decimal? Price { get; set; }
    }
}
