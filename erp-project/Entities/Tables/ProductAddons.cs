// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace erp_project.Entities.Tables
{
    [Table("ProductAddons", Schema = "productAndService")]
    public partial class ProductAddons
    {
        public ProductAddons()
        {
            ProductAddonDetails = new HashSet<ProductAddonDetails>();
        }

        [Key]
        public int AddonId { get; set; }
        public int ProductId { get; set; }
        [StringLength(100)]
        public string AddonImage { get; set; }
        public string AddonDescription { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal AddonPrice { get; set; }
        [Required]
        public bool? AddonStatus { get; set; }
        [Required]
        public bool? AddonActive { get; set; }
        [Required]
        [StringLength(100)]
        public string ProductCode { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(Products.ProductAddons))]
        public virtual Products Product { get; set; }
        [InverseProperty("Addon")]
        public virtual ICollection<ProductAddonDetails> ProductAddonDetails { get; set; }
    }
}