using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace erp_project.Libraries.Models.ProductAndService
{
    public class m_productandservice_request
    {

    }

    public class m_productandservice_main_request
    {
        public string productName { get; set; }
        public string productCode { get; set; }
        public int productTypeID { get; set; }
        public List<IFormFile> files { get; set; }
        public bool attributeStatus { get; set; }
        public int productUnitId { get; set; }
        public string description { get; set; }
        public int SellInfo { get; set; }
        public int domainId { get; set; }
        public List<m_productandservice_attributeName_request> attributeName { get; set; }
    }

    public class m_productandservice_attributeName_request
    {
        public int attributeNameId { get; set; }
        public string attributeName { get; set; }
        public List<m_productandservice_AttributeValue_request> Value { get; set; }
    }

    public class m_productandservice_AttributeValue_request 
    {
        public int attributeValueID { get; set; }
        public string AttributeValueName { get; set; }
        public List<IFormFile> files { get; set; }
        public bool Active { get; set; }
        public int SellingPrice { get; set; }
        public string Description { get; set; }
    }

    public class m_uploadimage
    {
        /// <summary>
        /// ชื่อเดิมของรูปภาพ
        /// </summary>
        public string OriginalName { get; set; }

        /// <summary>
        /// ชื่อใหม่ของรูปภาพ
        /// </summary>
        public string NewImageName { get; set; }

        /// <summary>
        /// ที่เก็บรูปภาพ
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// ที่เก็บรูปภาพพร้อมชื่อรูปภาพ
        /// </summary>
        public string fullPath { get; set; }

        /// <summary>
        /// ขนาดต่างๆ ของรูปภาพ
        /// </summary>
        public List<string> sizes { get; set; }
    }

}
