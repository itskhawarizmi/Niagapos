//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Niagapos
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.TransactionDetails = new HashSet<TransactionDetail>();
        }
    
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public int CurrentStock { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string ProductCategoryId { get; set; }
        public string ProductSupplierId { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransactionDetail> TransactionDetails { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual ProductSupplier ProductSupplier { get; set; }
    }
}
