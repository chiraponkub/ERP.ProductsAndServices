using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace erp_project.Libraries.Models.ProductAndService
{
    public class m_productandservice_response
    {
        public int productId { get; set; }
        public int productStatusId { get; set; }
        public int productTypeId { get; set; }
        public string productCode { get; set; }
        public string productName { get; set; }
        public string productDescription { get; set; }
        public int productUntiId { get; set; }
        public decimal productPrice { get; set; }
    }
}
