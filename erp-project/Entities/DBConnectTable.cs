﻿using erp_project.Entities.Tables;
using Microsoft.EntityFrameworkCore;

namespace erp_project.Entities
{
    /// <summary>
    /// สำหรับกำหนด Model ที่จะมาทำ DbSet ไว้ที่นี่
    /// </summary>
    public partial class DBConnect : DbContext
    {
        public virtual DbSet<Attribute> Attribute { get; set; }
        public virtual DbSet<BindAttribute> BindAttribute { get; set; }
        public virtual DbSet<BindGroupPrice> BindGroupPrice { get; set; }
        public virtual DbSet<GroupPrice> GroupPrice { get; set; }
        public virtual DbSet<MainProduct> MainProduct { get; set; }
        public virtual DbSet<ProductStatus> ProductStatus { get; set; }
        public virtual DbSet<ProductType> ProductType { get; set; }
        public virtual DbSet<ProductUnit> ProductUnit { get; set; }
    }
}
