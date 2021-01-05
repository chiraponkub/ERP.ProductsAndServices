using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace erp_project.Libraries.Models.PriceSetting
{

    public class m_priceSetting_response
    {
        public int groupPriceID { get; set; }
        public string priceName { get; set; }
        public string currencyCode { get; set; }
        public bool sellingPriceDefault { get; set; }
    }

    public class m_priceSetting_response_edit
    {
        public int groupPriceID { get; set; }
        public string priceName { get; set; }
    }

    public class m_priceSetting_GetDataPrice_response
    {
        public int ProductAttributeId { get; set; }
        public string productType { get; set; }
        public string productCode { get; set; }
        public string productName { get; set; }
        public string attribute { get; set; }
        public string productDescription { get; set; }
        public string productUnti { get; set; }
        public decimal? productPrice { get; set; }
        public string CurrencyCode { get; set; }
    }
}
