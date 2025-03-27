using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EntityLayer.Entities.Enums;

namespace EntityLayer.Entities
{
    public class SoldProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        private decimal _totalPrice;
        public decimal TotalPrice
        {
            get => UnitPrice * Quantity;
            set => _totalPrice = value;
        }
        public CurrencyType Currency { get; set; }
    }
}
