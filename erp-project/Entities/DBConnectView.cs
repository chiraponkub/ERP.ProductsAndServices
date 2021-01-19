using erp_project.Entities.Views;
using Microsoft.EntityFrameworkCore;

namespace erp_project.Entities
{
    /// <summary>
    /// สำหรับกำหนด Model ที่จะมาทำ DbSet ไว้ที่นี่
    /// </summary>
    public partial class DBConnect : DbContext
    {
        public virtual DbSet<GetDataAddon> GetDataAddon { get; set; }
        public virtual DbSet<GetDataAddonDetails> GetDataAddonDetails { get; set; }
        public virtual DbSet<GetDataAddonGroupPrice> GetDataAddonGroupPrice { get; set; }
        public virtual DbSet<GetProductAndServices> GetProductAndServices { get; set; }
    }
}
