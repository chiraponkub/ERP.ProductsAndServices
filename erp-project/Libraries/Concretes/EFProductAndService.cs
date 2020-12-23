using erp_project.Entities;
using erp_project.Entities.Tables;
using erp_project.Libraries.Abstracts;
using erp_project.Libraries.Models.ProductAndService;
using erp_project.Libraries.Models.Unit;
using erp_project.Middlewares;
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

                bool isShowCode = true;
                int orderByIndex = 0;
                try
                {
                    var product = new Products
                    {
                        ProductCode = req.productCode,
                        ProductName = req.productName,
                        ProductTypeId = req.productTypeId,
                        ProductDescription = req.productDescription,
                        ProductPrice = req.productPrice,
                        ProductImage = productimage,
                        ProductUntiId = req.productUntiId,
                        ProductActive = true,
                        DomianId = req.domainId,
                        ProductStatusId = req.productStatusId
                    };
                    db.Products.Add(product);
                    db.SaveChanges();

                    foreach (var m1 in req.attribute)
                    {
                        var attribute = new ProductAttributes
                        {
                            ProductId = product.ProductId,
                            AttibuteName = m1.attibuteName,
                            AttributeActive = true,
                        };
                        db.ProductAttributes.Add(attribute);
                        db.SaveChanges();

                        foreach (var m2 in m1.value)
                        {
                            var value = new ProductAttributeValues
                            {
                                AttributeId = attribute.AttributeId,
                                ValueName = m2.valueName,
                                ValueActive = true
                            };
                            db.ProductAttributeValues.Add(value);
                            db.SaveChanges();
                        }
                    }

                    foreach (var m3 in req.addon)
                    {
                        var addOn = new ProductAddons
                        {
                            ProductId = product.ProductId,
                            AddonImage = Attributeimage,
                            AddonDescription = m3.valueDescription,
                            AddonActive = true,
                            AddonStatus = m3.status,
                            AddonPrice = m3.price,
                            ProductCode = m3.productCodeOnValue,
                        };
                        db.ProductAddons.Add(addOn);
                        db.SaveChanges();
                    }
                    Transaction.Commit();

                    var tbl_attributes = db.ProductAttributes.Where(w => w.ProductId == product.ProductId).ToList();
                    var tbl_Addon = db.ProductAddons.Where(w => w.ProductId == product.ProductId).ToList();
                    var tbl_values = db.ProductAttributeValues.Where(w => tbl_attributes.Select(s => s.AttributeId).Contains(w.AttributeId)).ToList();
                    var res = (from proatt in tbl_attributes
                               where proatt.ProductId == product.ProductId
                               select new ProductAttributeModel
                               {
                                   AttrId = proatt.AttributeId,
                                   AttrName = proatt.AttibuteName,
                                   Values = (from provalue in tbl_values
                                             where provalue.AttributeId == proatt.AttributeId
                                             select new ProductAttributeValueModel
                                             {
                                                 ValueId = provalue.ValueId,
                                                 ValueName = provalue.ValueName
                                             }).ToList()
                               }).Distinct().ToList();
                    var retureProductList = ProductAddonExtesion.GetProductAddons(res).ToList();

                    var tbl_newAddon = tbl_Addon.Select(s => new
                    {
                        AddonId = s.AddonId,
                        ProductCodeList = string.Join("-", s.ProductCode.Split("-").Where(w => w != "code").ToList())
                    }).ToList();
                    foreach (var Product in retureProductList)
                    {
                        var keylist = Product.Keys;
                        List<m_ProductAttributeValueModels> ListAttributeStr = new List<m_ProductAttributeValueModels>();
                        foreach (var key in keylist)
                        {
                            if (key.ToLower() != "code")
                            {
                                var valuesObj = Product[key];
                                ProductAttributeValueModel m = (ProductAttributeValueModel)(Product[key].GetType().GetProperty("m").GetValue(Product[key], null));
                                var valueData = tbl_values.Where(w => w.ValueId == m.ValueId).SingleOrDefault();
                                var attributeData = tbl_attributes.Where(w => w.AttributeId == valueData.AttributeId).FirstOrDefault();
                                ListAttributeStr.Add(new m_ProductAttributeValueModels
                                {
                                    AttributeId = valueData.AttributeId,
                                    ValueId = valueData.ValueId,
                                    ValueName = valueData.ValueName,
                                    AttibuteName = attributeData.AttibuteName
                                });
                            }
                        }

                        int? tbl_AddonID = tbl_newAddon
                            .Where(w => w.ProductCodeList == Product["Code"]
                            .GetType()
                            .GetProperty("ValueName")
                            .GetValue(Product["Code"], null)
                            .ToString())
                            .FirstOrDefault()?.AddonId;

                        foreach (var AttributeStr in ListAttributeStr)
                        {
                            var saveaddon = new ProductAddonDetails
                            {
                                AddonId = tbl_AddonID.Value,
                                AttributeId = AttributeStr.AttributeId,
                                ValueId = AttributeStr.ValueId,
                                Active = true
                            };
                            db.ProductAddonDetails.Add(saveaddon);
                            db.SaveChanges();
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    Transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public class m_ProductAttributeValueModels
        {
            public int AttributeId { get; set; }
            public int ValueId { get; set; }
            public string ValueName { get; set; }
            public string AttibuteName { get; set; }
        }

        public bool delProductAndService(int ProductID)
        {
            using (var Transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var delproduct = db.Products.FirstOrDefault(f => f.ProductId == ProductID);
                    var delattributes = db.ProductAttributes.Where(w => w.ProductId == ProductID).ToList();
                    delproduct.ProductActive = false;
                    foreach (var m1 in delattributes)
                    {
                        m1.AttributeActive = false;
                        var delvalues = db.ProductAttributeValues.Where(w => w.AttributeId == m1.AttributeId).ToList();
                        foreach (var m2 in delvalues)
                        {
                            m2.ValueActive = false;
                        }
                        var deladdon = db.ProductAddons.Where(w => w.ProductId == m1.ProductId).ToList();
                        foreach (var m3 in deladdon)
                        {
                            m3.AddonActive = false;
                        }
                        db.SaveChanges();
                    }
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
