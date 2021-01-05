﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace erp_project.Entities.Tables
{
    [Table("Products", Schema = "productAndService")]
    public partial class Products
    {
        public Products()
        {
            ProductAddons = new HashSet<ProductAddons>();
            ProductAttributes = new HashSet<ProductAttributes>();
        }

        [Key]
        public int ProductId { get; set; }
        [StringLength(100)]
        public string ProductCode { get; set; }
        [Required]
        [StringLength(50)]
        public string ProductName { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductDescription { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ProductPrice { get; set; }
        [StringLength(50)]
        public string ProductImage { get; set; }
        public int ProductUntiId { get; set; }
        [Required]
        public bool? ProductActive { get; set; }
        public int DomainId { get; set; }
        [Column("ProductStatusID")]
        public int ProductStatusId { get; set; }

        [InverseProperty("Product")]
        public virtual ICollection<ProductAddons> ProductAddons { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<ProductAttributes> ProductAttributes { get; set; }
    }
}