using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CybSoftServices.Entities
{
    public class Server
    {
        public int ServerID { get; set; }
        public string Discription { get; set; }
        public string Services { get; set; }

        public int QTY { get; set; }
        public decimal Charge { get; set; }
        public decimal Total { get; set; }
        public DateTime ExpiringDate { get; set; }
        public string RAM { get; set; }
        public string HardDisk { get; set; }
        public string HDD_Used { get; set; }
        public string HDD_Available { get; set; }
        public string Access_Details { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}