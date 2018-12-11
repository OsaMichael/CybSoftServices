using CybSoftServices.Entities;
using CybSoftServices.Interface;
using CybSoftServices.Interface.Utils;
using CybSoftServices.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CybSoftServices.Manager
{
    public class ServiceManager : IServiceManager
    {
        private ApplicationDbContext _context;
        private IExcelProcessor _excel;
        public ServiceManager(ApplicationDbContext context, IExcelProcessor excel)
        {
            _context = context;
            _excel = excel;
        }
        public bool CreateService(ServiceModel model)
        {
            //return System.Operation.Create(() =>
            //{
               
                    var isExists = _context.Services.Where(c => c.ServId == model.ServId).FirstOrDefault();
                    if (isExists != null) throw new Exception("user email already exist");
                    var entity = model.Create(model);
                    _context.Services.Add(entity);
                _context.SaveChanges();
                return true;


            //});

        }
        public Operation<ServiceModel[]> GetServices()
        {
            return System.Operation.Create(() =>
            {
                var entities = _context.Services.ToList();

                var models = entities.Select(c => new ServiceModel(c)
                {
                     //ServId = c.ServId,
                    ServerDescription = c.ServerDescription,
                       Services = c.Services,
                       AlertExpired = c.AlertExpired,
                        Access_Details = c.Access_Details,
                          Email = c.Email,
                           CountDown = c.CountDown,
                    //get the date as inputed by the user
                    //ExpiringDate = c.ExpiringDate.ToString(),
                           ExpiringDate = c.ExpiringDate,
                           RenewerType = c.RenewerType,
                             CreatedBy = c.CreatedBy,
                              CreatedDate = DateTime.Now,
                               ModifiedDate = DateTime.Now
                }
                ).ToArray();
                return models;
            });
        }
        public Operation<ServiceModel> UpdateService(ServiceModel model)
        {
            return System.Operation.Create(() =>
            {
                //model.Validate();
                var isExist = _context.Services.Find(model.ServId);
                if (isExist == null) throw new Exception("service does not exist");

                var entity = model.Edit(isExist, model);
                _context.Entry(entity);
                _context.SaveChanges();
                return model;
            });
        }
        public Operation<ServiceModel> GetServiceById(int servId)
        {
            return System.Operation.Create(() =>
            {
                var entity = _context.Services.Where(c => c.ServId == servId).FirstOrDefault();
                if (entity == null) throw new Exception("service does not exist");
                return new ServiceModel(entity);

            });
        }
        public System.Operation DeleteService(int id)
        {
            return System.Operation.Create(() =>
            {
                var entity = _context.Services.Find(id);
                if (entity == null) throw new Exception("Voter does not exist");

                _context.Services.Remove(entity);
                _context.SaveChanges();
            });
        }

        public Operation<List<ServiceModel>> UploadServiceNames(Stream stream, ServiceModel model)
        {
            return Operation.Create(() =>
            {
                var sheet = _excel.Load<ServiceModel>(stream);
                var errors = new List<ServiceModel>();
                foreach (var row in sheet)
                {
                    // note: I check if staffNo exist in the database, if null, add the data and save it. if yes, edit the data and save it.
                    var service = _context.Services.Where(v => v.ServerDescription == row.ServerDescription && v.Services == row.Services && v.Access_Details == row.Access_Details && v.RenewerType == row.RenewerType && v.Email == row.Email && v.CountDown == row.CountDown && v.AlertExpired == row.AlertExpired).FirstOrDefault();
                    row.CreatedBy = model.CreatedBy;
                    row.ModifiedBy = model.ModifiedBy;
                    row.CreatedDate = DateTime.Now;

                    ////if (service != null) throw new Exception("Name already exist");
                    //////{

                    //if (row.ServerDescription == null || row.Services == null || row.Access_Details == null || row.ExpiringDate == null /*|| row.RenewerType == null || row.Email == null || row.CountDown == null || row.AlertExpired == null*/)
                    //{
                    //    throw new Exception("An Empty cell in the file");
                    //}
                    string date = row.ExpiringDate;
                    var voterEntity = new Service
                    {
                        CreatedBy = row.CreatedBy,
                        ModifiedBy = row.ModifiedBy,
                        CreatedDate = DateTime.Now,

                        ServerDescription = row.ServerDescription,
                         Access_Details = row.Access_Details,
                        Services = row.Services,
                        ExpiringDate = date,

                        RenewerType = row.RenewerType,
                        Email = row.Email,
                        CountDown = row.CountDown,
                        AlertExpired = row.AlertExpired,
                        //ModifiedDate  = DateTime.Now
                    };
                    _context.Services.Add(voterEntity);
                   
                }
                _context.SaveChanges();
                return errors;
            });
        }

    }
}