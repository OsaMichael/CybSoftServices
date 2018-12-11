using CybSoftServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CybSoftServices.Models
{
    public class ServerModel
    {
        public int ServerID { get; set; }
        public string Discription { get; set; }
        public string Services { get; set; }

        public int QTY { get; set; }
        public decimal Charge { get; set; }
        public decimal Total { get; set; }
        public string ExpiringDate { get; set; }
        public string RAM { get; set; }
        public string HardDisk { get; set; }
        public string HDD_Used {get; set; }
        public string HDD_Available { get; set; }
        public string Access_Details { get; set; }
        public string Message { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; } = DateTime.Now;

        public ServerModel()
        {
            
        }
        public ServerModel(Server server)
        {
            this.Assign(server);
        }
        public Server Create(ServerModel model)
        {
            return new Server
            {
                 Access_Details = model.Access_Details,
                  Charge = model.Charge,
                   Discription = model.Discription,
                    ExpiringDate = DateTime.Parse(model.ExpiringDate),
                     HardDisk = model.HardDisk,
                      HDD_Available = model.HDD_Available,
                       HDD_Used = model.HDD_Used,
                        QTY = model.QTY,
                         RAM = model.RAM,
                           Services =  model.Services,
                            Total = model.Total,
                              CreatedBy = model.CreatedBy,
                                CreatedDate = DateTime.Now,
            };        
        }
        public Server Edit( Server entity , ServerModel model)
        {
          
            entity.Access_Details = model.Access_Details;
            entity.Charge = model.Charge;
            entity.Discription = model.Discription;
            entity.ExpiringDate = DateTime.Parse( model.ExpiringDate);
            entity.HardDisk = model.HardDisk;
            entity.HDD_Available = model.HDD_Available;
            entity.HDD_Used = model.HDD_Used;
            entity.QTY = model.QTY;
            entity.RAM = model.RAM;
            entity.ServerID = model.ServerID;
            entity.Services = model.Services;
            entity.Total = model.Total;
            entity.ModifiedBy = model.ModifiedBy;      
            entity.ModifiedDate = DateTime.Now;
            return entity;

           
        }
    }
}