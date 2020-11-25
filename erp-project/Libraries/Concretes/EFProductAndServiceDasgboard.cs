using erp_project.Entities;
using erp_project.Libraries.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace erp_project.Libraries.Concretes
{
    public class EFProductAndServiceDasgboard : IProductAndServiceDasgboard
    {
        private readonly DBConnect db;

        public EFProductAndServiceDasgboard(DBConnect db)
        {
            this.db = db;
        }

    }
}
