﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace erp_project.Entities.Tables
{
    [Table("mainProduct", Schema = "productAndService")]
    public partial class MainProduct
    {
        [Key]
        [Column("mainProductID")]
        public int MainProductId { get; set; }
        [Required]
        [Column("productName")]
        [StringLength(100)]
        public string ProductName { get; set; }
        [Column("productCode")]
        [StringLength(100)]
        public string ProductCode { get; set; }
        [Column("productTypeID")]
        public int ProductTypeId { get; set; }
        [Column("productImage")]
        [StringLength(50)]
        public string ProductImage { get; set; }
        [Column("productStatus")]
        public bool ProductStatus { get; set; }
        [Column("productUnitID")]
        public int ProductUnitId { get; set; }
        [Column("productActive")]
        public bool ProductActive { get; set; }
        [Required]
        [Column("description")]
        public string Description { get; set; }
        [Column("productSellInfo")]
        public int ProductSellInfo { get; set; }
        [Column("domainID")]
        public int DomainId { get; set; }
    }
}