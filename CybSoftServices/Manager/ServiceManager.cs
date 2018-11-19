using CybSoftServices.Interface;
using CybSoftServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CybSoftServices.Manager
{
    public class ServiceManager : IServiceManager
    {
        private ApplicationDbContext _context;

        public ServiceManager(ApplicationDbContext context)
        {
            _context = context;
        }
        public Operation<ServiceModel> CreateService(ServiceModel model)
        {
            return System.Operation.Create(() =>
            {
                //try
                //{
                    var isExists = _context.Services.Where(c => c.ServId == model.ServId).FirstOrDefault();
                    if (isExists != null) throw new Exception("user email already exist");
                    var entity = model.Create(model);
                    _context.Services.Add(entity);
                _context.SaveChanges();

                //}
                //catch (Exception xe)
                //{
                //    throw xe;
                //}
                //model.Validate();

                return model;


            });

        }
        public Operation<ServiceModel[]> GetServices()
        {
            return System.Operation.Create(() =>
            {
                var entities = _context.Services.ToList();

                var models = entities.Select(c => new ServiceModel(c)
                {
                   
                      Name = c.Name,
                       Description = c.Description,
                        AlertExpired = c.AlertExpired,
                        // CountDown = c.CountDown,
                          Email = c.Email,
                           CountDown = c.CountDown,
                          //get the date as inputed by the user
                           ExpiredDate = c.ExpiredDate,
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
    }
}