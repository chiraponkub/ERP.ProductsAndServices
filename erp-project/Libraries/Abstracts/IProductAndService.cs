using erp_project.Libraries.Models.ProductAndService;
using erp_project.Libraries.Models.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace erp_project.Libraries.Abstracts
{
    public interface IProductAndService
    {
        bool addProductAndService(m_productandservice_main_request req, string productimage, string Attributeimage);
        bool delProductAndService(int ProductID);
        bool unit(m_unit_request res);
        public bool editunit(int untiId, m_unit_edit_request res);
        public bool delunit(int untiId);
    }
}
