using CybSoftServices.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CybSoftServices.Models
{
    public class ServiceModel
    {
        public int ServId { get; set; }
        public string UserId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ExpiredDate { get; set; }
        public string RenewerType { get; set; }
        //  public int CountDown { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public int CountDown { get; set; }
        [Required]
        public int AlertExpired { get; set; }
        public string Message { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; } = DateTime.Now;

        public virtual ApplicationUser User { get; set; }

        public ServiceModel()
        {
            new ApplicationUser();
        }
        public ServiceModel(Service services)
        {
            this.Assign(services);
            new ApplicationUser();

        }
        public Service Create(ServiceModel model)
        {
            return new Service
            {
                UserId = model.UserId,

                Name = model.Name,
                Description = model.Description,
                RenewerType = model.RenewerType,
               AlertExpired = model.AlertExpired,
                CountDown = model.CountDown,
                Email = model.Email,
                //get the date as inputed by the user
                ExpiredDate = DateTime.Parse(model.ExpiredDate),
                //ExpiredDate = DateTime.AddDays(Duration),
                CreatedBy = model.CreatedBy,
                CreatedDate = DateTime.Now,
                      //ModifiedBy = model.ModifiedBy,
                      // ModifiedDate = DateTime.Now 

            };
        }
        public Service Edit(Service entity, ServiceModel model)
        {
            entity.UserId = model.UserId;
            entity.ServId = model.ServId;
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.RenewerType = model.RenewerType;
           entity.AlertExpired = model.AlertExpired;
            entity.Email = model.Email;
            entity.CountDown = model.CountDown;
            entity.ExpiredDate = DateTime.Parse(model.ExpiredDate);
            //entity.ExpiredDate = DateTime.Now.AddDays(Duration);
            entity.ModifiedBy = model.ModifiedBy;
            //entity.CreatedBy = model.CreatedBy;
            entity.ModifiedDate = DateTime.Now;
            //entity.CreatedDate = DateTime.Now;
            return entity;
        }
    }
}