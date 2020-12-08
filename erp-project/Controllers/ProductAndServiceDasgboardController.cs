using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using erp_project.Entities;
using erp_project.Libraries.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace erp_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAndServiceDasgboardController : ERPControllerBase
    {
        private readonly DBConnect db;
        private readonly IProductAndServiceDasgboard IProductAndServiceDasgboard;

        public ProductAndServiceDasgboardController(DBConnect db , IProductAndServiceDasgboard IProductAndServiceDasgboard)
        {
            this.db = db;
            this.IProductAndServiceDasgboard = IProductAndServiceDasgboard;
        }

        [Authorize]
        [HttpGet("getunit")]
        public IActionResult getunit(int domainId)
        {
            try
            {
                return Ok(IProductAndServiceDasgboard.getunit(domainId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("editunit")]
        public IActionResult editunit(int unitId)
        {
            try
            {
                return Ok(IProductAndServiceDasgboard.editunit(unitId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    } 
}
