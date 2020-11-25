using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using erp_project.Entities;
using erp_project.Libraries.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace erp_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAndServiceController : ERPControllerBase
    {

        private readonly DBConnect db;
        private readonly IProductAndService IProductAndService;

        public ProductAndServiceController(DBConnect db , IProductAndService IProductAndService)
        {
            this.db = db;
            this.IProductAndService = IProductAndService;
        }
    }
}
