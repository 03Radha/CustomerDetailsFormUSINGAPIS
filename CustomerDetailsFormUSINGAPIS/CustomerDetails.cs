using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerDetailsFormUSINGAPIS
{
    internal class CustomerDetails
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string AliasName { get; set; }
        public string CustomerType { get; set; }
        public string MobileAlert { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string BankName { get; set; }
        public string Fax { get; set; }
        public string CustCode { get; set; }
        public int CreditDays { get; set; }
        public decimal CrDrBalance { get; set; }
        public decimal CreditLimit { get; set; }
        public DateTime? ModifyOn { get; set; }
        public string Address { get; set; }
        public string AreaName { get; set; }
        public string PlaceCity { get; set; }
        public string PostalCode { get; set; }
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public string OffPhone1 { get; set; }
        public string OffPhone2 { get; set; }
        public string Mobile { get; set; }
        public string PanCardNo { get; set; }
        public string AadharCardNo { get; set; }
        public string GST { get; set; }
        public string FoodLicenceNo { get; set; }
        public string Other1 { get; set; }
        public string Other2 { get; set; }
        public string TypeOfDealer { get; set; }
       // public string Picture { get; set; }
    }
}
