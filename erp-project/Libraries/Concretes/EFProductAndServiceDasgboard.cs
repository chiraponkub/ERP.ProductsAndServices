using erp_project.Entities;
using erp_project.Libraries.Abstracts;
using erp_project.Libraries.Models.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using erp_project.Middlewares;
using erp_project.Libraries.Models.ProductAndService;
using Microsoft.EntityFrameworkCore;
using erp_project.Libraries.Models.PriceSetting;
using erp_project.Entities.Tables;
using Microsoft.Data.SqlClient;

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
            string domainId,
            string StatusId,
            string Type,
            string ProductCode,
            string ProductName,
            string Description,
            string Unit,
            decimal? Above,
            decimal? Below
            )
        {
            List<m_productandservice_response> models = new List<m_productandservice_response>();
            var Products = db.Products.Where(f => f.DomainId.ToString() == domainId && f.ProductActive == true);
            if (
                string.IsNullOrEmpty(StatusId)
                && string.IsNullOrEmpty(Type)
                && string.IsNullOrEmpty(ProductCode)
                && string.IsNullOrEmpty(ProductName)
                && string.IsNullOrEmpty(Description)
                && string.IsNullOrEmpty(Unit)
                && Above == null
                && Below == null
                )
            {
                var res = Products.ToList();
                foreach (var m1 in res)
                {
                    models.Add(new m_productandservice_response
                    {
                        productId = m1.ProductId,
                        productStatusId = m1.ProductStatusId,
                        productTypeId = m1.ProductTypeId,
                        productCode = m1.ProductCode,
                        productName = m1.ProductName,
                        productDescription = m1.ProductDescription,
                        productUntiId = m1.ProductUntiId,
                        productPrice = m1.ProductPrice
                    });
                }
            }
            else if (Above == null && Below == null)
            {
                var res = Products.Where(w =>
                w.ProductStatusId.ToString().Contains(StatusId)
                || w.ProductTypeId.ToString().Contains(Type)
                || w.ProductCode.Contains(ProductCode)
                || w.ProductName.Contains(ProductName)
                || w.ProductDescription.Contains(Description)
                || w.ProductUntiId.ToString().Contains(Unit)
                ).ToList();
                foreach (var m1 in res)
                {
                    models.Add(new m_productandservice_response
                    {
                        productId = m1.ProductId,
                        productStatusId = m1.ProductStatusId,
                        productTypeId = m1.ProductTypeId,
                        productCode = m1.ProductCode,
                        productName = m1.ProductName,
                        productDescription = m1.ProductDescription,
                        productUntiId = m1.ProductUntiId,
                        productPrice = m1.ProductPrice
                    });
                }
            }
            else
            {
                if (Above != null && Below == null)
                {
                    var res = Products.Where(w => w.ProductPrice >= Above)
                        .Where(w => w.ProductStatusId.ToString().Contains(StatusId)
                        || w.ProductTypeId.ToString().Contains(Type)
                        || w.ProductCode.Contains(ProductCode)
                        || w.ProductName.Contains(ProductName)
                        || w.ProductDescription.Contains(Description)
                        || w.ProductUntiId.ToString().Contains(Unit))
                        .ToList();
                    foreach (var m1 in res)
                    {
                        models.Add(new m_productandservice_response
                        {
                            productId = m1.ProductId,
                            productStatusId = m1.ProductStatusId,
                            productTypeId = m1.ProductTypeId,
                            productCode = m1.ProductCode,
                            productName = m1.ProductName,
                            productDescription = m1.ProductDescription,
                            productUntiId = m1.ProductUntiId,
                            productPrice = m1.ProductPrice
                        });
                    }
                }
                else if (Below != null && Above == null)
                {
                    var res = Products.Where(w => w.ProductPrice <= Below)
                        .Where(w => w.ProductStatusId.ToString().Contains(StatusId)
                        || w.ProductTypeId.ToString().Contains(Type)
                        || w.ProductCode.Contains(ProductCode)
                        || w.ProductName.Contains(ProductName)
                        || w.ProductDescription.Contains(Description)
                        || w.ProductUntiId.ToString().Contains(Unit))
                        .ToList();
                    foreach (var m1 in res)
                    {
                        models.Add(new m_productandservice_response
                        {
                            productId = m1.ProductId,
                            productStatusId = m1.ProductStatusId,
                            productTypeId = m1.ProductTypeId,
                            productCode = m1.ProductCode,
                            productName = m1.ProductName,
                            productDescription = m1.ProductDescription,
                            productUntiId = m1.ProductUntiId,
                            productPrice = m1.ProductPrice
                        });
                    }
                }
                else
                {
                    var res = Products.Where(w => w.ProductPrice >= Above && w.ProductPrice <= Below)
                        .Where(w => w.ProductStatusId.ToString().Contains(StatusId)
                        || w.ProductTypeId.ToString().Contains(Type)
                        || w.ProductCode.Contains(ProductCode)
                        || w.ProductName.Contains(ProductName)
                        || w.ProductDescription.Contains(Description)
                        || w.ProductUntiId.ToString().Contains(Unit))
                        .ToList();
                    foreach (var m1 in res)
                    {
                        models.Add(new m_productandservice_response
                        {
                            productId = m1.ProductId,
                            productStatusId = m1.ProductStatusId,
                            productTypeId = m1.ProductTypeId,
                            productCode = m1.ProductCode,
                            productName = m1.ProductName,
                            productDescription = m1.ProductDescription,
                            productUntiId = m1.ProductUntiId,
                            productPrice = m1.ProductPrice
                        });
                    }
                }
            }
            return models;
        }

        public List<m_priceSetting_GetDataPrice_response> GetDataPrice(
            int domainId,
            int GroupPriceId,
            string Type,
            string ProductCode,
            string ProductName,
            string Attribute,
            string Description,
            string Unit,
            decimal? Above,
            decimal? Below
            )
        {
            var addon = db.ProductAddons;
            var product = db.Products;
            var groupPrice = db.GroupPrice.FirstOrDefault(f => f.GroupPriceId == GroupPriceId);
            var DataAddon = db.GetDataAddon.Where(w => w.DomainId == domainId).ToList();

            foreach (var m1 in DataAddon)
            {
                var check = db.BindGroupPrice.Where(w => w.AddonId == m1.AddonId && w.GroupPriceId == GroupPriceId && w.Active == true && w.DomainId == domainId).FirstOrDefault();
                if (check == null)
                {
                    var save = new BindGroupPrice
                    {
                        AddonId = m1.AddonId,
                        GroupPriceId = GroupPriceId,
                        DomainId = domainId,
                        Price = m1.AddonPrice,
                        Active = true
                    };
                    db.BindGroupPrice.Add(save);
                    db.SaveChanges();
                }
            }
            string AddonPrice = $" AND AddonPrice IS NOT NULL AND DomainId = {domainId}";
            if (Above != null && Below != null)
            {
                AddonPrice += $" AND AddonPrice BETWEEN {Above} AND {Below}";
            }
            else if (Above != null && Below == null)
            {
                AddonPrice += $" AND AddonPrice >= {Above}";
            }
            else if (Above == null && Below != null)
            {
                AddonPrice += $" AND AddonPrice <= {Below}";
            }

            var retrue = db.GetDataAddon.FromSqlRaw($"Exec [productAndService].[AdvancedSearchAddOn] @Type,@ProductCode,@ProductName,@Attribute,@Description,@Unit,@AddonPrice",
                new SqlParameter("@Type", Type ?? (object)DBNull.Value),
                new SqlParameter("@ProductCode", ProductCode ?? (object)DBNull.Value),
                new SqlParameter("@ProductName", ProductName ?? (object)DBNull.Value),
                new SqlParameter("@Attribute", Attribute ?? (object)DBNull.Value),
                new SqlParameter("@Description", Description ?? (object)DBNull.Value),
                new SqlParameter("@Unit", Unit ?? (object)DBNull.Value),
                new SqlParameter("@AddonPrice", AddonPrice ?? (object)DBNull.Value)
                ).ToList();

            var BindGroupPrice = db.BindGroupPrice.Where(w => w.GroupPriceId == GroupPriceId).ToList();
            var show = (from b in BindGroupPrice
                        join a in retrue on b.AddonId equals a.AddonId
                        select new m_priceSetting_GetDataPrice_response
                        {
                            ProductAttributeId = b.AddonId,
                            productType = a.ProductTypeName,
                            productCode = a.ProductCode,
                            productName = a.ProductName,
                            attribute = a.Attribute,
                            productDescription = a.AddonDescription,
                            productUnti = a.UnitCode,
                            productPrice = b.Price,
                            CurrencyCode = groupPrice.CurrencyCode
                        }).ToList();
            return show;
        }


        public List<m_priceSetting_response> getprice(int domainId)
        {

            var res = db.GroupPrice.Where(w => w.DomainId == domainId && w.Active == true).ToList();
            List<m_priceSetting_response> models = new List<m_priceSetting_response>();
            foreach (var m1 in res)
            {
                models.Add(new m_priceSetting_response
                {
                    groupPriceID = m1.GroupPriceId,
                    priceName = m1.PriceName,
                    currencyCode = m1.CurrencyCode,
                    sellingPriceDefault = m1.SellingPriceDefault
                });
            }
            return models;
        }

        public m_priceSetting_response_edit getEditPrice(int groupPriceID)
        {
            var res = db.GroupPrice.Where(w => w.Active == true).FirstOrDefault(f => f.GroupPriceId == groupPriceID);
            if (res == null)
                throw new Exception("No ID!!!");
            m_priceSetting_response_edit models = new m_priceSetting_response_edit();
            models.groupPriceID = res.GroupPriceId;
            models.priceName = res.PriceName;
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
