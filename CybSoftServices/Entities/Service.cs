using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CybSoftServices.Entities
{
    public partial class Service
    {
        [Key]
        public int ServId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ExpiredDate { get; set; }
        public string RenewerType { get; set; }
      //  public int CountDown { get; set; }
        public int CountDown { get; set; }
        public string Email { get; set; }
        public int AlertExpired { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}