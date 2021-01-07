using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace erp_project.Libraries.Models.PriceSetting
{
    public class m_priceSetting_request
    {
        public string priceName { get; set; }
        public int currencyId { get; set; }
        public string currencyCode { get; set; }
        public int domainID { get; set; }
    }

    public class m_priceSetting_request_edti
    {
        public string priceName { get; set; }
    }

    public class PrimaryCurrency
    {
        public int PrimaryCurrencyId { get; set; }
        public string PrimaryCurrencycode { get; set; }
        public string PrimaryCurrencyNameEn { get; set; }
        public string PrimaryCurrencyNameTh { get; set; }
    }

    public partial class Domain
    {
        public Domain()
        {
            domainModule = new HashSet<DomainModule>();
            domainUserOrder = new HashSet<DomainUserOrder>();
            userRoleDomain = new HashSet<UserRoleDomain>();
        }

        public int domainId { get; set; }
        public string domainCode { get; set; }
        public string domainName { get; set; }
        public int documentLanguageId { get; set; }
        public int primaryCurrencyId { get; set; }
        public string billimgCycle { get; set; }
        public string companyName { get; set; }
        public string taxId { get; set; }
        public string domainAddress { get; set; }
        public string zipCode { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string companyImage { get; set; }
        public Guid? globalUserId { get; set; }
        public bool isTrial { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? expireDate { get; set; }
        public string domainStatus { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime? updatedAt { get; set; }

        public virtual ICollection<DomainModule> domainModule { get; set; }
        public virtual ICollection<DomainUserOrder> domainUserOrder { get; set; }
        public virtual ICollection<UserRoleDomain> userRoleDomain { get; set; }
    }

    public class DomainModule 
    { 
    
    }

    public class DomainUserOrder 
    { 
    
    }

    public class UserRoleDomain 
    { 
    
    }



    public class EditPrice 
    {
        public int ProductAttributeId { get; set; }
        public decimal? productPrice { get; set; }
    }
}
