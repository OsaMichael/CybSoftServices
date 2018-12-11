using CybSoftServices.Entities;
using CybSoftServices.Interface;
using CybSoftServices.Interface.Utils;
using CybSoftServices.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CybSoftServices.Manager
{
    public class ServerManager : IServerManager
    {

        private ApplicationDbContext _context;
        private IExcelProcessor _excel;
        public ServerManager(ApplicationDbContext context, IExcelProcessor excel)
        {
            _context = context;
            _excel = excel;
        }
        public bool CreateServer(ServerModel model)
        {
            //return System.Operation.Create(() =>
            //{

            var isExists = _context.Servers.Where(c => c.ServerID == model.ServerID).FirstOrDefault();
            if (isExists != null) throw new Exception("user email already exist");
            var entity = model.Create(model);
            _context.Servers.Add(entity);
            _context.SaveChanges();
            return true;


            //});

        }
        public Operation<ServerModel[]> GetServers()
        {
            return System.Operation.Create(() =>
            {
                var entities = _context.Servers.ToList();

                var models = entities.Select(c => new ServerModel(c)
                {

                    Discription = c.Discription,
                     Access_Details = c.Access_Details,
                     Charge = c.Charge,
                    ExpiringDate = c.ExpiringDate.ToString(),
                     HardDisk = c.HardDisk,
                     HDD_Available = c.HDD_Available,
                     HDD_Used = c.HDD_Used,
                       QTY = c.QTY,
                        RAM = c.RAM,
                         Services = c.Services,
                          Total = c.Total,
                                                             
                    CreatedBy = c.CreatedBy,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                }
                ).ToArray();
                return models;
            });
        }
        public Operation<ServerModel> UpdateServer(ServerModel model)
        {
            return System.Operation.Create(() =>
            {
                //model.Validate();
                var isExist = _context.Servers.Find(model.ServerID);
                if (isExist == null) throw new Exception("service does not exist");

                var entity = model.Edit(isExist, model);
                _context.Entry(entity);
                _context.SaveChanges();
                return model;
            });
        }
        public Operation<ServerModel> GetServerById(int serverId)
        {
            return System.Operation.Create(() =>
            {
                var entity = _context.Servers.Where(c => c.ServerID == serverId).FirstOrDefault();
                if (entity == null) throw new Exception("service does not exist");
                return new ServerModel(entity);

            });
        }
        public System.Operation DeleteServer(int id)
        {
            return System.Operation.Create(() =>
            {
                var entity = _context.Servers.Find(id);
                if (entity == null) throw new Exception("Voter does not exist");

                _context.Servers.Remove(entity);
                _context.SaveChanges();
            });
        }

        public Operation<List<ServerModel>> UploadServerNames(Stream stream, ServerModel model)
        {
            return Operation.Create(() =>
            {
                var sheet = _excel.Load<ServerModel>(stream);
                var errors = new List<ServerModel>();
                foreach (var row in sheet)
                {
                    // note: I check if staffNo exist in the database, if null, add the data and save it. if yes, edit the data and save it.
                    var service = _context.Servers.Where(v => v.Discription == row.Discription && v.Access_Details == row.Access_Details && v.Charge == row.Charge && v.HardDisk == row.HardDisk && v.HDD_Available == row.HDD_Available && v.HDD_Used == row.HDD_Used && v.QTY == row.QTY && v.RAM == row.RAM && v.Services == row.Services && v.Total == row.Total ).FirstOrDefault();
                    row.CreatedBy = model.CreatedBy;
                    row.ModifiedBy = model.ModifiedBy;
                    row.CreatedDate = DateTime.Now;

                    ////if (service != null) throw new Exception("Name already exist");
                    //////{

                    //if (row.Discription == null || row.Access_Details == null || row.Charge == null || row.HardDisk == null || row.HDD_Available == null || row.HDD_Used == null || row.QTY == null || row.RAM == null || row.Services == null || row.Total == null)
                    //{
                    //    throw new Exception("An Empty cell in the file");
                    //}
                    string date = row.ExpiringDate;
                    var voterEntity = new Server
                    {
                        CreatedBy = row.CreatedBy,
                        ModifiedBy = row.ModifiedBy,
                        CreatedDate = DateTime.Now,


                        Discription = row.Discription,
                        ExpiringDate = DateTime.ParseExact(date, "dd/MM/yyyy", null),
                        Access_Details = row.Access_Details,
                        Charge = row.Charge,
                        HardDisk = row.HardDisk,
                        HDD_Available = row.HDD_Available,
                        HDD_Used = row.HDD_Used,
                         QTY = row.QTY,
                          Total = row.Total,
                           Services = row.Services,
                            RAM = row.RAM
                    };
                    _context.Servers.Add(voterEntity);

                }
                _context.SaveChanges();
                return errors;
            });
        }

    }
}