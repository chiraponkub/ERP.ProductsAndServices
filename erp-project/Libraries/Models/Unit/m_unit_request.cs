using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace erp_project.Libraries.Models.Unit
{
    public class m_unit_request
    {
        public int domainID { get; set; }
        public string unitName { get; set; }
    }

    public class m_unit_edit_request
    {
        public string unitName { get; set; }
    }

}
