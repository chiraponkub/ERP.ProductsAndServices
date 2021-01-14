using erp_project.Entities;
using erp_project.Entities.Tables;
using erp_project.Libraries.Abstracts;
using erp_project.Libraries.Models.PriceSetting;
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

        public int addProductAndService(m_productandservice_main_request req, string productimage, List<string> Attributeimage)
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
                        DomainId = req.domainId,
                        ProductStatusId = req.productStatusId
                    };
                    db.Products.Add(product);
                    db.SaveChanges();
                    var Images = db.ProductAddons.Where(f => f.ProductId == product.ProductId).ToList();

                    if (req.attribute == null || req.addon == null)
                    {
                        Transaction.Commit();
                        return product.ProductId;
                    }

                    foreach (var m1 in req.attribute)
                    {
                        var attribute = new ProductAttributes
                        {
                            ProductId = product.ProductId,
                            AttibuteName = m1.attibuteName,
                            AttributeActive = true
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
                            AddonImage = null,
                            AddonDescription = m3.valueDescription,
                            AddonActive = true,
                            AddonStatus = m3.status,
                            AddonPrice = m3.price,
                            ProductCode = m3.productCodeOnValue
                        };
                        db.ProductAddons.Add(addOn);
                        db.SaveChanges();
                    }
                    int count1 = 0;
                    foreach (var mm1 in Images)
                    {
                        int count2 = 0;
                        if (Attributeimage != null)
                        {
                            foreach (var image in Attributeimage)
                            {
                                if (count1 == count2)
                                {
                                    var es = db.ProductAddons.FirstOrDefault(f => f.AddonId == mm1.AddonId);
                                    es.AddonImage = image;
                                    db.SaveChanges();
                                    break;
                                }
                            }
                            count2++;
                        }
                        count1++;
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


                        var tbl_newAddon = tbl_Addon.Select(s => new
                        {
                            AddonId = s.AddonId,
                            ProductCodeList = string.Join("", s.ProductCode.Split(product.ProductCode + "-").Where(w => w != product.ProductCode).ToList())
                        }).ToList();

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
                    return product.ProductId;
                }
                catch (Exception ex)
                {
                    Transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }
        public bool editProductAndSerivce(Edit_productandservice_main_request req, string productimage, List<string> Attributeimage, int ProductsId)
        {
            using (var Transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var Find = db.Products.FirstOrDefault(f => f.ProductId == ProductsId);
                    if (Find == null)
                    {
                        throw new Exception("ProductId Not Found");
                    }
                    Find.ProductName = req.productName;
                    Find.ProductTypeId = req.productTypeId;
                    Find.ProductCode = req.productCode;
                    Find.ProductImage = string.IsNullOrEmpty(productimage) ? Find.ProductImage : productimage;
                    Find.ProductStatusId = req.productStatusId;
                    Find.ProductUntiId = req.productUntiId;
                    Find.ProductDescription = req.productDescription;
                    Find.ProductPrice = req.productPrice;
                    db.SaveChanges();
                    if (req.attribute == null || req.addon == null)
                    {
                        var att = db.ProductAttributes.Where(w => w.ProductId == Find.ProductId).ToList();
                        if (att != null)
                        {
                            foreach (var m1 in att)
                            {
                                m1.AttributeActive = false;
                                db.SaveChanges();

                                var val = db.ProductAttributeValues.Where(w => w.AttributeId == m1.AttributeId).ToList();
                                foreach (var m2 in val)
                                {
                                    m2.ValueActive = false;
                                    db.SaveChanges();
                                }

                                var addon = db.ProductAddons.Where(w => w.ProductId == Find.ProductId).ToList();
                                foreach (var m3 in addon)
                                {
                                    m3.AddonActive = false;
                                    db.SaveChanges();
                                }
                            }
                        }
                        Transaction.Commit();
                        return true;
                    }
                    else
                    {
                        var att = db.ProductAttributes.Where(w => w.ProductId == Find.ProductId).ToList();
                        if (att != null)
                        {
                            foreach (var m1 in att)
                            {
                                m1.AttributeActive = false;
                                db.SaveChanges();

                                var val = db.ProductAttributeValues.Where(w => w.AttributeId == m1.AttributeId).ToList();
                                foreach (var m2 in val)
                                {
                                    m2.ValueActive = false;
                                    db.SaveChanges();
                                }

                                var addon = db.ProductAddons.Where(w => w.ProductId == Find.ProductId).ToList();
                                foreach (var m3 in addon)
                                {
                                    m3.AddonActive = false;
                                    db.SaveChanges();
                                }
                            }
                        }

                        foreach (var m1 in req.attribute)
                        {
                            var attribute = new ProductAttributes
                            {
                                ProductId = Find.ProductId,
                                AttibuteName = m1.attibuteName,
                                AttributeActive = true
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
                                ProductId = Find.ProductId,
                                AddonImage = null,
                                AddonDescription = m3.valueDescription,
                                AddonActive = true,
                                AddonStatus = m3.status,
                                AddonPrice = m3.price,
                                ProductCode = m3.productCodeOnValue
                            };
                            db.ProductAddons.Add(addOn);
                            db.SaveChanges();

                            //if (Attributeimage != null)
                            //{
                            //    var ss = db.ProductAddons.FirstOrDefault(f => f.AddonId == addOn.AddonId);
                            //    int count1 = 1;
                            //    foreach (var image in Attributeimage)
                            //    {
                            //        if (m3.files.Count() == 1 && req.addon[count1] == count1)
                            //        {
                            //            var es = db.ProductAddons.FirstOrDefault(f => f.AddonId == addOn.AddonId);
                            //            es.AddonImage = image;
                            //            db.SaveChanges();
                            //        }
                            //        count1++;
                            //    }
                            //}



                            //if (Attributeimage != null)
                            //{
                            //    foreach (var img in Attributeimage)
                            //    {
                            //        var ProductAddons = db.ProductAddons.FirstOrDefault(f => f.AddonId == addOn.AddonId);
                            //        ProductAddons.AddonImage = img;
                            //        db.SaveChanges();
                            //    }
                            //}
                        }
                        var Images = db.ProductAddons.Where(f => f.ProductId == Find.ProductId && f.AddonActive == true).ToList();
                        int count1 = 0;

                        foreach (var mm1 in Images)
                        {
                            int count2 = 0;
                            if (Attributeimage != null)
                            {
                                foreach (var image in Attributeimage)
                                {
                                    if (count1 == count2)
                                    {
                                        var es = db.ProductAddons.FirstOrDefault(f => f.AddonId == mm1.AddonId);
                                        es.AddonImage = image;
                                        db.SaveChanges();
                                        break;
                                    }
                                }
                                count2++;
                            }
                            count1++;
                        }

                        Transaction.Commit();

                        var tbl_attributes = db.ProductAttributes.Where(w => w.ProductId == Find.ProductId && w.AttributeActive == true).ToList();
                        var tbl_Addon = db.ProductAddons.Where(w => w.ProductId == Find.ProductId && w.AddonActive == true).ToList();
                        var tbl_values = db.ProductAttributeValues.Where(w => w.ValueActive == true && tbl_attributes.Select(s => s.AttributeId).Contains(w.AttributeId)).ToList();
                        var res = (from proatt in tbl_attributes
                                   where proatt.ProductId == Find.ProductId
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


                            var tbl_newAddon = tbl_Addon.Select(s => new
                            {
                                AddonId = s.AddonId,
                                ProductCodeList = string.Join("", s.ProductCode.Split(Find.ProductCode + "-").Where(w => w != Find.ProductCode).ToList())
                            }).ToList();

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
        public bool addPrice(m_priceSetting_request req)
        {
            using (var Transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var check = db.GroupPrice.Where(w => w.DomainId == req.domainID).ToList();
                    foreach (var m1 in check)
                    {
                        if (m1.PriceName == req.priceName && m1.CurrencyCode == req.currencyCode && m1.Active == true)
                        {
                            throw new Exception("ชื่อซ้ำกับชื่อราคาซ้ำ");
                        }
                    }
                    var save = db.GroupPrice.Add(new GroupPrice
                    {
                        PriceName = req.priceName,
                        CurrencyId = req.currencyId,
                        CurrencyCode = req.currencyCode,
                        DomainId = req.domainID
                    });
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
        public bool editPrice(int GroupPriceId, m_priceSetting_request_edti req)
        {
            using (var Transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var save = db.GroupPrice.FirstOrDefault(f => f.GroupPriceId == GroupPriceId);
                    if (save == null)
                    {
                        throw new Exception("Error Id Fales");
                    }
                    save.PriceName = req.priceName;
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
        public bool delPrice(int GroupPriceId)
        {
            using (var Transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var save = db.GroupPrice.FirstOrDefault(f => f.GroupPriceId == GroupPriceId);
                    if (save == null)
                    {
                        throw new Exception("Error Id Fales");
                    }
                    save.Active = false;
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
        public bool EditaddonPrice(int GroupPriceId, List<EditPrice> req)
        {
            foreach (var m1 in req)
            {
                var check = db.GroupPrice.Where(w => w.GroupPriceId == GroupPriceId && w.SellingPriceDefault == true).FirstOrDefault();
                if (check != null)
                {
                    var res = db.ProductAddons.FirstOrDefault(f => f.AddonId == m1.ProductAttributeId);
                    res.AddonPrice = m1.productPrice;
                    db.SaveChanges();
                }
                else
                {
                    var res = db.BindGroupPrice.FirstOrDefault(f => f.AddonId == m1.ProductAttributeId && f.GroupPriceId == GroupPriceId);
                    res.Price = m1.productPrice;
                    db.SaveChanges();
                }
            }
            return true;
        }
        public bool unit(m_unit_request res)
        {
            var check = db.ProductUnit.Where(f => f.DomainId == res.domainID).ToList();
            if (check != null)
            {
                foreach (var m1 in check)
                {
                    if (m1.UnitCode == res.unitName)
                    {
                        throw new Exception("ชื่อซ้ำ");
                    }
                }
                ProductUnit ss = new ProductUnit
                {
                    DomainId = res.domainID,
                    UnitCode = res.unitName
                };
                db.ProductUnit.Add(ss);
                db.SaveChanges();
            }
            return true;
        }
        public bool editunit(int untiId, m_unit_edit_request res)
        {
            var Find = db.ProductUnit.FirstOrDefault(f => f.ProductUnitId == untiId);
            if (Find == null)
                return false;
            if (Find.UnitCode == res.unitName && Find.Active == true)
                throw new Exception("ชื่อซ้ำ");
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
