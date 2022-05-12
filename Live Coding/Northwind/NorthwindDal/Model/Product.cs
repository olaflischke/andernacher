using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NorthwindDal.Model
{
    [Index("CategoryId", Name = "CategoriesProducts")]
    [Index("CategoryId", Name = "CategoryID")]
    [Index("ProductName", Name = "ProductName")]
    [Index("SupplierId", Name = "SupplierID")]
    [Index("SupplierId", Name = "SuppliersProducts")]
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(40)]
        public string ProductName { get; set; } = null!;
        [Column("SupplierID")]
        public int? SupplierId { get; set; }
        [Column("CategoryID")]
        public int? CategoryId { get; set; }
        [StringLength(20)]
        public string? QuantityPerUnit { get; set; }
        [Column(TypeName = "money")]
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
        public byte[]? Timestamp { get; set; }

        [InverseProperty("Product")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
