using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entities
{
    public class Enums
    {
        public enum CurrencyType
        {
            RON = 1,
            EUR = 2,
            USD = 3
        }

        public enum InvoiceType
        {
            Normal = 1,
            RO_FATURA = 2,
            C = 3
        }

        public enum TransactionType
        {
            Income = 1,
            Expense = 2
        }
    }
}
