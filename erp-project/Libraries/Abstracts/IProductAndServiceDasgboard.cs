using erp_project.Libraries.Models.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace erp_project.Libraries.Abstracts
{
    public interface IProductAndServiceDasgboard
    {
        List<m_unit_response> getunit(int domainID);
        m_unit_response editunit(int unitId);
    }
}
