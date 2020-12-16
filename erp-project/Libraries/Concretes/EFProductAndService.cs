using erp_project.Entities;
using erp_project.Entities.Tables;
using erp_project.Libraries.Abstracts;
using erp_project.Libraries.Models.ProductAndService;
using erp_project.Libraries.Models.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace erp_project.Libraries.Concretes
{
    public class EFProductAndService : IProductAndService
    {
        private readonly DBConnect db;

        public EFProductAndService(DBConnect db)
        {
            this.db = db;
        }

        public bool addProductAndService(m_productandservice_main_request req, string productimage, string Attributeimage)
        {
            using (var Transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var product = new MainProduct
                    {
                        DomainId = req.domainId,
                        ProductName = req.productName,
                        ProductTypeId = req.productTypeID,
                        ProductCode = req.productCode,
                        ProductStatus = req.attributeStatus,
                        ProductUnitId = req.productUnitId,
                        Description = req.description,
                        ProductActive = true,
                        ProductSellInfo = req.SellInfo,
                        ProductImage = productimage
                    };
                    db.MainProduct.Add(product);
                    db.SaveChanges();

                   
                    Transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    Transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }


        public bool delProductAndService(int mainProductID)
        {
            using (var Transaction = db.Database.BeginTransaction())
            {
                try
                {
                    

                    Transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    Transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public bool unit(m_unit_request res)
        {
            ProductUnit ss = new ProductUnit
            {
                DomainId = res.domainID,
                UnitCode = res.unitName
            };
            db.ProductUnit.Add(ss);
            db.SaveChanges();
            return true;
        }

        public bool editunit(int untiId, m_unit_edit_request res)
        {
            var Find = db.ProductUnit.FirstOrDefault(f => f.ProductUnitId == untiId);
            if (Find == null)
            {
                return false;
            }
            Find.UnitCode = res.unitName;
            db.SaveChanges();
            return true;
        }

        public bool delunit(int untiId)
        {
            var Find = db.ProductUnit.FirstOrDefault(f => f.ProductUnitId == untiId);
            if (Find == null)
            {
                return false;
            }
            Find.Active = false;
            db.SaveChanges();
            return true;
        }
    }
}
