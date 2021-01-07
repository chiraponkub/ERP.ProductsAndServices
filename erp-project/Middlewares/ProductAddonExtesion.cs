using erp_project.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
namespace erp_project.Middlewares
{
    public static class ProductAddonExtesion
    {
        /// <summary>
        /// ส่วนเอาไว้กระจาย ProductAttribute ออกมาเป็นส่วนๆ (ทวีคูณ Product attribute)
        /// </summary>
        /// <param name="productAttribute">Product attribute ต้องแปลงมาเป็น Object นี้ก่อน</param>
        /// <param name="orderByIndex">เรียงข้อมูลจาก Index ที่</param>
        /// <param name="isShowMeta"></param>
        /// <param name="isShowCode"></param>
        /// <returns></returns>
        public static IEnumerable<Dictionary<string, object>> GetProductAddons(this IEnumerable<ProductAttributeModel> productAttribute,
            int orderByIndex = 0,
            bool isShowMeta = false,
            bool isShowCode = true
            )
        {
            var items = new List<Dictionary<string, object>>();
            // วนลูปข้อมูลเพื่อนำมาใส่ที่ Dictionary
            foreach (var attr in productAttribute)
            {
                // เก็บ Column เพื่อเอามาทำ คีย์
                var columnName = attr.AttrName;
                var columnNameID = attr.AttrId.ToString();
                // เก็บค่าเริ่มต้นใส่ Dictionary ครั้งแรก
                if (items.Count <= 0)
                {
                    attr.Values.ToList().ForEach(m =>
                    {
                        items.Add(new Dictionary<string, object> { { columnName, new { m.ValueName , m}  } });
                    });
                }
                // เก็บค่าครั้งถัดไปหลังจากที่ Dictionary มีข้อมูลเริ่มต้นแล้ว
                else
                {
                    // Loop ตรวจสอบเพื่อกระขายค่าไปที่ Dictionary แต่ละตัวทั้งที่มีอยู่แล้ว และที่เข้ามาใหม่
                    items.ToList().ForEach(item =>
                    {
                        attr.Values.ToList().ForEach(m =>
                        {
                            // หากเป็นข้อมูลเก่าที่มีอยู่แล้วก็ใส่ข้อมูลใหม่เข้าไป
                            if (!item.ContainsKey(columnName))
                            {
                                item.Add(columnName, new { m.ValueName , m });
                            }
                            // หากเป็นข้อมูลใหม่ที่เพิ่มเข้ามาก็ใส่ข้อมูลเก่าเข้าไปให้กับตัวใหม่
                            else
                            {
                                var newItem = new Dictionary<string, object>();
                                item.ToList().ForEach(_item =>
                                {
                                    if (_item.Key.Contains(columnName)) return;
                                    // เพิ่มข้อมูลเก่า
                                    newItem.Add(_item.Key, _item.Value);
                                });
                                // เพิ่มข้อมูลใหม่
                                newItem.Add(columnName, new { m.ValueName, m });
                                items.Add(newItem);
                            }
                        });
                    });
                }
            }
            // หากว่ามีข้อมูลก็เรียงข้อมูลใหม่โดยใช้ orderByIndex Main ในการเรียงข้อมูล (Default ไว้คือตัวแรกของ Dictionary)
            if (items.Count > 0)
            {
                // หากต้องการดึง Meta data
                if (isShowMeta) items.ForEach(item =>
                {
                    var metadataItems = new List<Dictionary<string, object>>();
                    item.ToList().ForEach(arr =>
                    {
                        foreach (var m1 in item)
                        {
                            metadataItems.Add(new Dictionary<string, object>
                        {
                            { "Column", arr.Key },
                            { "Value", arr.Value  }
                        });
                        }
                    });
                    item.Add("_meta", metadataItems);
                });
                // ดึงข้อมูล Product Code
                if (isShowCode) items.ForEach(item =>
                {
                    var Codes = new List<string>();
                    item.ToList().ForEach(arr => {
                        Codes.Add(arr.Value.GetType().GetProperty("ValueName").GetValue(arr.Value, null).ToString().Replace(" ", "-"));
                        }
                    );
                    item.Add("Code", new { ValueName = string.Join("-", Codes), m = new ProductAttributeValueModel() { ValueId = -1 } });
                });
                return items.OrderBy(m => m[items[0]
                .ToArray()[orderByIndex].Key]
                .GetType()
                .GetProperty("ValueName")
                .GetValue(m[items[0]
                .ToArray()[orderByIndex].Key],null)
                .ToString());
            }
            return items;
        }

    }

    public class ProductAttributeModel
    {
        public string ProductCode { get; set; }
        public int AttrId { get; set; }
        public string AttrName { get; set; }
        public IEnumerable<ProductAttributeValueModel> Values { get; set; }
    }

    public class ProductAttributeValueModel
    {
        public int ValueId { get; set; }
        public string ValueName { get; set; }
    }
}