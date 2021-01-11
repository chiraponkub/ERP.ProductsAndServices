using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace erp_project.Libraries.Models.ProductAndService
{
    public class m_productandservice_response
    {
        public int productId { get; set; }
        public int? productStatusId { get; set; }
        public int? productTypeId { get; set; }
        public string productCode { get; set; }
        public string productName { get; set; }
        public string productDescription { get; set; }
        public int productUntiId { get; set; }
        public decimal productPrice { get; set; }
    }


    public class m_Edit_productandservice_main_request
    {
        public int productsId { get; set; }
        public string productCode { get; set; }
        public string productName { get; set; }
        public int productTypeId { get; set; }
        public string productDescription { get; set; }
        public decimal productPrice { get; set; }
        public string files { get; set; }
        public int productUntiId { get; set; }
        public int productStatusId { get; set; }
        public int domainId { get; set; }
        public List<m_Edit_productandservice_attributeName_request> attribute { get; set; }
        public List<m_Edit_productandservice_Addon_request> addon { get; set; }
    }

    public class m_Edit_productandservice_attributeName_request
    {
        public string attibuteName { get; set; }
        public List<m_Edit_productandservice_AttributeValue_request> value { get; set; }
    }

    public class m_Edit_productandservice_AttributeValue_request
    {
        public string valueName { get; set; }
    }

    public class m_Edit_productandservice_Addon_request
    {
        public string files { get; set; }
        public string valueDescription { get; set; }
        public decimal price { get; set; }
        public bool status { get; set; }
        public string productCodeOnValue { get; set; }
    }

}
