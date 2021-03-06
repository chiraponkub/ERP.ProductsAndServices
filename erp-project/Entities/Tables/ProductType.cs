// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace erp_project.Entities.Tables
{
    [Table("productType", Schema = "productAndService")]
    public partial class ProductType
    {
        [Key]
        [Column("productTypeID")]
        public int ProductTypeId { get; set; }
        [Column("productTypeName")]
        [StringLength(50)]
        public string ProductTypeName { get; set; }
        [Column("productStatusID")]
        public int? ProductStatusId { get; set; }
    }
}