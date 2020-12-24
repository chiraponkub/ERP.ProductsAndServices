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
        public string productCode { get; set; }
        public string productName { get; set; }
        public int productTypeId { get; set; }
        public string productDescription { get; set; }
        public int productPrice { get; set; }
        //public List<IFormFile> files { get; set; }
        public int productUntiId { get; set; }
        public int productStatusId { get; set; }
        public int domainId { get; set; }
        public bool productActive { get; set; }
        public List<m_productandservice_attributeName_request> attribute { get; set; }
        public List<m_productandservice_Addon_request> addon { get; set; }
    }

    public class m_productandservice_attributeName_request
    {
        public string attibuteName { get; set; }
        public List<m_productandservice_AttributeValue_request> value { get; set; }
    }

    public class m_productandservice_AttributeValue_request
    {
        public string valueName { get; set; }
    }

    public class m_productandservice_Addon_request
    {
        //public List<IFormFile> files { get; set; }
        public string valueDescription { get; set; }
        public int price { get; set; }
        public bool status { get; set; }
        public string productCodeOnValue { get; set; }
    }

    public class m_ProductAttributeValueModels
    {
        public int AttributeId { get; set; }
        public int ValueId { get; set; }
        public string ValueName { get; set; }
        public string AttibuteName { get; set; }
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
