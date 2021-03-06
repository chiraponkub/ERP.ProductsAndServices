// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace erp_project.Entities.Tables
{
    [Table("ProductAttributeValues", Schema = "productAndService")]
    public partial class ProductAttributeValues
    {
        [Key]
        public int ValueId { get; set; }
        public int AttributeId { get; set; }
        [Required]
        [StringLength(50)]
        public string ValueName { get; set; }
        [Required]
        public bool? ValueActive { get; set; }

        [ForeignKey(nameof(AttributeId))]
        [InverseProperty(nameof(ProductAttributes.ProductAttributeValues))]
        public virtual ProductAttributes Attribute { get; set; }
    }
}