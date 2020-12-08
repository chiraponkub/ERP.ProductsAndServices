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

        /// <summary>
        /// ดึงข้อมูล Unit ทั้งหมดของ Domain นั้น
        /// </summary>
        /// <param name="domainId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// ดึงข้อมูล Unit มาเพื่อแก้ไข
        /// </summary>
        /// <param name="unitId"></param>
        /// <returns></returns>
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
