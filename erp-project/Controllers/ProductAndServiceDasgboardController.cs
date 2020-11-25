using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using erp_project.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace erp_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAndServiceDasgboardController : ERPControllerBase
    {
        private readonly DBConnect db;

        public ProductAndServiceDasgboardController(DBConnect db)
        {
            this.db = db;
        }


    }
}
