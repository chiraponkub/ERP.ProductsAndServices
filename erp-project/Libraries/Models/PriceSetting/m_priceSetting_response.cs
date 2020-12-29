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
        public int currencyCode { get; set; }
        public bool sellingPriceDefault { get; set; }
    }

    public class m_priceSetting_response_edit
    {
        public int groupPriceID { get; set; }
        public string priceName { get; set; }
    }

    public class m_priceSetting_Product_response 
    { 
        
    }
}
