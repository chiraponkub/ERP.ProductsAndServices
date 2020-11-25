using erp_project.Entities;
using erp_project.Libraries.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace erp_project.Libraries.Concretes
{
    public class EFProductAndService : IProductAndService
    {
        private readonly DBConnect db;

        public EFProductAndService(DBConnect db)
        {
            this.db = db;
        }


    }
}
