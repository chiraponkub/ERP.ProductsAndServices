using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using erp_project.Entities;
using erp_project.Entities.Tables;
using erp_project.Libraries.Abstracts;
using erp_project.Libraries.Models.PriceSetting;
using erp_project.Library.Concretes;
using erp_project.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace erp_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAndServiceDasgboardController : ERPControllerBase
    {
        private readonly DBConnect db;
        private readonly IProductAndServiceDasgboard IProductAndServiceDasgboard;

        public ProductAndServiceDasgboardController(DBConnect db, IProductAndServiceDasgboard IProductAndServiceDasgboard)
        {
            this.db = db;
            this.IProductAndServiceDasgboard = IProductAndServiceDasgboard;
        }

        /// <summary>
        /// ดึงข้อมูล ProductAndService
        /// </summary>
        /// <param name="domainId">ส่ง DomainId</param>
        /// <param name="StatusId">ส่ง ProductStatusId</param>
        /// <param name="Type">ส่ง ProdcutTypeId</param>
        /// <param name="ProductCode">ส่ง ProductCode</param>
        /// <param name="ProductName">ชื่อสินค้า</param>
        /// <param name="Description">รายละเอียดสินค้า</param>
        /// <param name="Unit">ส่ง UnitId</param>
        /// <param name="Above">ราคามากกว่า</param>
        /// <param name="Below">ราคาน้อยกว่า</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("Dasgboard{domainId}")]
        public ActionResult getproduct(
            string domainId,
            string StatusId,
            string Type,
            string ProductCode,
            string ProductName,
            string Description,
            string Unit,
            decimal? Above,
            decimal? Below
            )
        {
            try
            {
                return Ok(IProductAndServiceDasgboard.GetProdtuct(domainId,
                    StatusId,
                    Type,
                    ProductCode,
                    ProductName,
                    Description,
                    Unit,
                    Above,
                    Below
                    ));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// ดึงข้อมูล Price มาแสดง
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("getPrice{domainId}")]
        public IActionResult getPrice(int domainId)
        {
            try
            {
                string Token = UserAuthorization;
                string host = Configuration.GetValue<string>("BE_HOST");
                var httpservice = new HttpApiService();
                httpservice.Authorization(Token);
                Domain dos = new Domain();
                var getdomain = httpservice.Get<ERPHttpResponse<Domain>>($"{host}/rest-account/api/Domain/GetDomainById?domainId={domainId}").Result.Content;
                var Find = db.GroupPrice.Where(w => w.SellingPriceDefault == true && w.DomainId == domainId && w.Active == true).FirstOrDefault();

                var PrimaryCurrency = HttpService.Get<ERPHttpResponse<List<PrimaryCurrency>>>($"{host}/rest-master/api/Master/currency").Result.Content;
                PrimaryCurrency documentLanguage = PrimaryCurrency.data.Where(w => w.PrimaryCurrencyId == getdomain.data.primaryCurrencyId).FirstOrDefault();

                if (Find == null)
                {
                    var save = new GroupPrice
                    {
                        PriceName = "Standard",
                        CurrencyId = getdomain.data.primaryCurrencyId,
                        CurrencyCode = documentLanguage.PrimaryCurrencycode,
                        DomainId = domainId,
                        SellingPriceDefault = true
                    };
                    db.GroupPrice.Add(save);
                    db.SaveChanges();
                }
                return Ok(IProductAndServiceDasgboard.getprice(domainId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// ดึงข้อมูล Price มาแก้ไข
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("getEditPrice{GroupPriceId}")]
        public IActionResult getEditPrice(int GroupPriceId)
        {
            try
            {
                return Ok(IProductAndServiceDasgboard.getEditPrice(GroupPriceId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("GetDataPrice{GroupPriceId},{domainId}")]
        public IActionResult GetDataPrice(
            int domainId,
            int GroupPriceId,
            string Type,
            string ProductCode,
            string ProductName,
            string Attribute,
            string Description,
            string Unit,
            decimal? Above,
            decimal? Below
            )
        {
            try
            {
                return Ok(IProductAndServiceDasgboard.GetDataPrice(
                    domainId,
                    GroupPriceId,
                    Type,
                    ProductCode,
                    ProductName,
                    Attribute,
                    Description,
                    Unit,
                    Above,
                    Below
                    ));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
