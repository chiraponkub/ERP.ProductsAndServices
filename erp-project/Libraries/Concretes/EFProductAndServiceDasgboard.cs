using erp_project.Entities;
using erp_project.Libraries.Abstracts;
using erp_project.Libraries.Models.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using erp_project.Middlewares;
using erp_project.Libraries.Models.ProductAndService;

namespace erp_project.Libraries.Concretes
{
    public class EFProductAndServiceDasgboard : IProductAndServiceDasgboard
    {
        private readonly DBConnect db;

        public EFProductAndServiceDasgboard(DBConnect db)
        {
            this.db = db;
        }

        public List<m_productandservice_response> GetProdtuct(
            int domainId,
            int StatusId,
            int Type,
            string ProductCode,
            string ProductName,
            string Description,
            int Unit,
            decimal Above,
            decimal Below,
            decimal Between
            )
        {
            List<m_productandservice_response> models = new List<m_productandservice_response>();
            var Products = db.Products.Where(f => f.DomianId == domainId);

            var select = (
                from p in Products
                where p.ProductStatusId.ToString().Contains(StatusId.ToString()) 
                || p.ProductTypeId.ToString().Contains(Type.ToString())
                || p.ProductCode.Contains(ProductCode)
                || p.ProductDescription.Contains(Description)
                || p.ProductUntiId.ToString().Contains(Unit.ToString())
                || p.ProductPrice >= Above 
                || p.ProductPrice <= Below
                && p.ProductPrice >= Between && p.ProductPrice <= Between
                select new m_productandservice_response() 
                { 
                    productStatusId = p.ProductStatusId
                }).ToList();
            
            foreach (var m1 in select)
            {
                models.Add(new m_productandservice_response
                {
                    productId = m1.productId,
                    productStatusId = m1.productStatusId,
                    productTypeId = m1.productTypeId,
                    productCode = m1.productCode,
                    productName = m1.productName,
                    productDescription = m1.productDescription,
                    productUntiId = m1.productUntiId,
                    productPrice = m1.productPrice
                });
            }
            return models;
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
