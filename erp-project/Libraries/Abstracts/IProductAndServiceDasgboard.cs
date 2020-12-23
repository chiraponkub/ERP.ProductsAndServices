using erp_project.Libraries.Models.ProductAndService;
using erp_project.Libraries.Models.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace erp_project.Libraries.Abstracts
{
    public interface IProductAndServiceDasgboard
    {
        List<m_productandservice_response> GetProdtuct(
            int domainId,
            int StatusId,
            int Type,
            string ProductCode,
            string ProductName,
            string Description,
            int Unit,
            decimal Above,
            decimal Below,
            decimal Between);
        List<m_unit_response> getunit(int domainID);
        m_unit_response editunit(int unitId);
    }
}
