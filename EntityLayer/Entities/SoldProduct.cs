﻿using System;
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
        public decimal TotalPrice => UnitPrice * Quantity;
        public CurrencyType Currency { get; set; }
    }
}
