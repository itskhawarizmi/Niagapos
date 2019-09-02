using System;
using Niagapos.Core;

namespace Niagapos
{
    public class TransactionDetailViewModel
    {
        public string TransactionDetailId { get; set; }
        public string TransactionId { get; set; }
        public string CustomerId { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string CustomerName { get; set; }
        public string Bill { get; set; } = "TO1901001";
        public string DateAndTime { get; set; } = DateTime.Now.ToLocalTime().ToString("yyyy:MM:dd - hh:mm:ss");
        public string User { get; set; } = "Administrator";
        public string ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPayItem { get; set; }
        public int PurchaseTotal { get; set; }
        public string Note { get; set; } = "Tidak ada catatan untuk transaksi saat ini";
        public decimal Total { get; set; }
        public decimal Change { get; set; }
        public int Quantity { get; set; }
        public decimal Disc { get; set; } 
        public decimal Cash { get; set; }
    }
}
