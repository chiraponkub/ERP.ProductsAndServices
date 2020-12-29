﻿using erp_project.Libraries.Models.PriceSetting;
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
            string domainId,
            string StatusId,
            string Type,
            string ProductCode,
            string ProductName,
            string Description,
            string Unit,
            decimal? Above,
            decimal? Below
            );

        List<m_priceSetting_response> getprice(int domainId);
        m_priceSetting_response_edit getEditPrice(int groupPriceID);
        List<m_unit_response> getunit(int domainID);
        m_unit_response editunit(int unitId);
    }
}
