using erp_project.Entities;
using erp_project.Libraries.Abstracts;
using erp_project.Libraries.Models.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace erp_project.Libraries.Concretes
{
    public class EFProductAndServiceDasgboard : IProductAndServiceDasgboard
    {
        private readonly DBConnect db;

        public EFProductAndServiceDasgboard(DBConnect db)
        {
            this.db = db;
        }

        public List<m_unit_response> getunit(int domainID)
        {
            var res = db.ProductUnit.Where(w => w.DomainId == domainID && w.Active == true).ToList();
            List<m_unit_response> models = new List<m_unit_response>();
            foreach (var m1 in res)
            {
                models.Add(new m_unit_response
                {
                    unitId = m1.ProductUnitId,
                    unitName = m1.UnitCode
                });
            }
            return models;
        }

        public m_unit_response editunit(int unitId)
        {
            var m1 = db.ProductUnit.Where(w => w.Active == true).FirstOrDefault(w => w.ProductUnitId == unitId);
            if (m1 == null)
            {
                throw new Exception("UnitId Not Found");
            }
            m_unit_response models = new m_unit_response();
            models.unitId = m1.ProductUnitId;
            models.unitName = m1.UnitCode;
            return models;
        }
    }
}
