using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace erp_project.Models.MakeProduct
{
    public class m_MakeProduct
    {
        public string attibuteName { get; set; }
        public List<m_MakeProduct_valueName> value { get; set; }
    }

    public class m_MakeProduct_valueName
    {
        public string valueName { get; set; }
    }
}
