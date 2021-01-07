using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using erp_project.Entities;
using erp_project.Libraries.Abstracts;
using erp_project.Libraries.Models.PriceSetting;
using erp_project.Libraries.Models.ProductAndService;
using erp_project.Libraries.Models.Unit;
using erp_project.Middlewares;
using erp_project.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace erp_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAndServiceController : ERPControllerBase
    {

        private readonly DBConnect db;
        private readonly IProductAndService IProductAndService;

        public ProductAndServiceController(DBConnect db, IProductAndService IProductAndService)
        {
            this.db = db;
            this.IProductAndService = IProductAndService;
        }



        /// <summary>
        /// เพิ่ม ProductAndService (ยังไม่สามารถเพิ่มรูปภาพได้)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("addProductAndService")]
        public ActionResult AddProductAndService([FromForm] m_productandservice_main_request req)
        {
            try
            {
                string image;
                ERPHttpResponse<List<m_uploadimage>> p_image = new ERPHttpResponse<List<m_uploadimage>>();
                ERPHttpResponse<List<m_uploadimage>> List_image = new ERPHttpResponse<List<m_uploadimage>>();
                if (req.files != null && req.files.Count() > 0)
                {
                    if (req.files.Count() > 1)
                        return BadRequest("ไม่สามารถอัพรูปโปรไฟล์ได้มากว่า 1 รูป");

                    HttpService.Authorization(UserAuthorization);
                    string host = Configuration.GetValue<string>("BE_HOST");
                    //p_image = HttpService.PostFile<ERPHttpResponse<List<m_uploadimage>>>($"https://localhost:5004/api/Upload/Uploadimg", req.files).Result.Content;
                    p_image = HttpService.PostFile<ERPHttpResponse<List<m_uploadimage>>>($"{host}/rest-resource/api/Upload/Uploadimg", req.files).Result.Content;
                    if (p_image.message != "Ok" || p_image.data.Count() != 1)
                        return BadRequest("ไม่สามารถ บันทึกรูปภาพได้");
                    image = p_image.data[0].fullPath;

                    List<string> Attributeimage = new List<string>();
                    foreach (m_productandservice_Addon_request m1 in req.addon)
                    {
                        if (m1.files != null && m1.files.Count() > 0)
                        {

                            List<IFormFile> files = new List<IFormFile>();

                            IFormFile adasd = m1.files[0];

                            Stream Streamfile = adasd.OpenReadStream();

                            IFormFile ss = new FormFile(Streamfile, 0, Streamfile.Length, "files", adasd.FileName)
                            {
                                Headers = adasd.Headers
                            };
                            files.Add(ss);

                            List_image = HttpService.PostFile<ERPHttpResponse<List<m_uploadimage>>>($"{host}/rest-resource/api/Upload/Uploadimg", files).Result.Content;
                            //List_image = HttpService.PostFile<ERPHttpResponse<List<m_uploadimage>>>($"https://localhost:5004/api/Upload/Uploadimg", files).Result.Content;
                            if (List_image.message != "Ok" || List_image.data.Count() != 1)
                                return BadRequest("ไม่สามารถ บันทึกรูปภาพได้");
                            Attributeimage.Add(List_image.data[0].fullPath);
                        }
                    }
                    return Ok(IProductAndService.addProductAndService(req, image, Attributeimage));
                }
                return Ok(IProductAndService.addProductAndService(req, null, null));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        

        /// <summary>
        /// ตัวอย่างรูปแบบการส่งข้อมูลแบบ Json ใน AddProduct
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("JsonAddProduct")]
        public ActionResult JsonAddProduct(m_productandservice_main_request req)
        {
            m_productandservice_main_request models = new m_productandservice_main_request();
            return Ok(models);
        }

        /// <summary>
        /// ส่วนเอาไว้กระจาย ProductAttribute ออกมาเป็นส่วนๆ (ทวีคูณ Product attribute)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("MakeProduct")]
        public ActionResult MakeProduct(IEnumerable<ProductAttributeModel> req)
        {
            try
            {
                var retureProductList = ProductAddonExtesionToFE.GetProductAddons(req);
                return Ok(retureProductList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// ลบ ProductAndService
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("delPraductAndService{ProductID}")]
        public ActionResult delProductAndService(int ProductID)
        {
            try
            {
                return Ok(IProductAndService.delProductAndService(ProductID));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// เพิ่ม PriceSetting
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("addPrice")]
        public ActionResult PriceSetting(m_priceSetting_request req)
        {
            try
            {
                return Ok(IProductAndService.addPrice(req));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// แก้ไข PriceSettingName
        /// </summary>
        /// <param name="GroupPriceId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("EditPrice{GroupPriceId}")]
        public ActionResult EditPrice(int GroupPriceId, m_priceSetting_request_edti req)
        {
            try
            {
                return Ok(IProductAndService.editPrice(GroupPriceId, req));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// ลบ PriceSettingName
        /// </summary>
        /// <param name="GroupPriceId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("DeltPrice{GroupPriceId}")]
        public ActionResult DeltPrice(int GroupPriceId)
        {
            try
            {
                return Ok(IProductAndService.delPrice(GroupPriceId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// สร้าง Unit 
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
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

        /// <summary>
        /// แก้ไข Unit
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="res"></param>
        /// <returns></returns>
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

        /// <summary>
        /// ลบ Unit
        /// </summary>
        /// <param name="unitId"></param>
        /// <returns></returns>
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
