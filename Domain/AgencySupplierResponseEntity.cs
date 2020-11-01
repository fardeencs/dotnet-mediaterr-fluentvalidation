namespace Domain
{
    using System;
    public class AgencySupplierResponseEntity
    {
        public long SearchID { get; set; }
        public long AgencyID { get; set; }
        public string AgencyCode { get; set; }
        public DateTime SearchDateTime { get; set; }      
        public string Search { get; set; }
        public int? Status { get; set; }
        public string SupplierCode { get; set; }
        public string Key { get; set; }
    }
}
