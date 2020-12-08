using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using erp_project.Entities;
using erp_project.Libraries.Abstracts;
using erp_project.Libraries.Models.Unit;
using Microsoft.AspNetCore.Authorization;
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


        [Authorize]
        [HttpPost("Unit")]
        public ActionResult Unit(m_unit_request res)
        {
            try
            {
                return Ok(IProductAndService.unit(res));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("editUnit")]
        public ActionResult editUnit(int unitId, m_unit_edit_request res)
        {
            try
            {
                return Ok(IProductAndService.editunit(unitId, res));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("delUnit")]
        public ActionResult delUnit(int unitId)
        {
            try
            {
                return Ok(IProductAndService.delunit(unitId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
