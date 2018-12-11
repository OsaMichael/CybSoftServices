using CybSoftServices.Entities;
using CybSoftServices.Interface;
using CybSoftServices.Interface.Utils;
using CybSoftServices.Models;
using nVisionIT.ExcelProcessor.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CybSoftServices.Manager
{
    public class UploadManager : IUploadManager
    {
        private IDataRepository _db;
        private IExcelProcessor _excel;
        private IServiceManager _servMgr;
        public UploadManager(IDataRepository db, IExcelProcessor excel, IServiceManager servMgr)
        {
            _db = db;
            _excel = excel;
            _servMgr = servMgr;
        }


        public Operation<List<ServiceModel>> UploadServiceNames(Stream stream, ServiceModel model)
        {
            return Operation.Create((Func<List<ServiceModel>>)(() =>
            {
                var sheet = _excel.Load<ServiceModel>(stream);
                var errors = new List<ServiceModel>();
                foreach (var row in sheet)
                {
                    var servNm = _db.Query<Service>().Where(v => v.ServId == row.ServId).FirstOrDefault();
                    // var votername = _votgr.GetVoters().Result.Where(v => v.StaffName == model.StaffName).FirstOrDefault();
                    if (servNm == null)
                    {
                        row.Message = "service does not exist";
                        errors.Add(row);
                        continue;
                    }
                    else
                    {
                        row.Message = "service does not exist";
                    }
                    row.CreatedBy = model.CreatedBy;
                    row.ModifiedBy = model.ModifiedBy;
                    row.ServerDescription = model.ServerDescription;
                    row.Services = model.Services;
                    row.Email = model.Email;
                    row.ExpiringDate = row.ExpiringDate /*== staffNm.StaffName ? row.StaffName : staffNm.StaffName*/;
                    row.RenewerType = row.RenewerType;
                    row.CountDown = row.CountDown;
                    row.Access_Details = row.Access_Details/*== staffNm.StaffNo ? row.StaffNo : staffNm.StaffNo*/;
               
                    if (servNm == null)
                    {
                        var entity = row.Create(row);
                        _db.Add(entity);
                    }
                    else
                    {
                        var entity = row.Edit(servNm, row);
                        _db.Update(entity);
                    }
                    var result = _db.SaveChanges();
                    if (result.Succeeded == false)
                    {
                        row.Message = result.Message;
                        errors.Add(row);
                    }
                }
                return errors;
            }));


        }
        // the below was not used
        public Operation<List<ServerModel>> UploadServerNames(Stream stream, ServerModel model)
        {
            return Operation.Create((Func<List<ServerModel>>)(() =>
            {
                var sheet = _excel.Load<ServerModel>(stream);
                var errors = new List<ServerModel>();
                foreach (var row in sheet)
                {
                    var servNm = _db.Query<Server>().Where(v => v.ServerID == row.ServerID).FirstOrDefault();
                    // var votername = _votgr.GetVoters().Result.Where(v => v.StaffName == model.StaffName).FirstOrDefault();
                    if (servNm == null)
                    {
                        row.Message = "service does not exist";
                        errors.Add(row);
                        continue;
                    }
                    else
                    {
                        row.Message = "service does not exist";
                    }
                    row.CreatedBy = model.CreatedBy;
                    row.ModifiedBy = model.ModifiedBy;
                    row.Discription = model.Discription;
                    row.Services = model.Services;
                    row.QTY = row.QTY /*== staffNm.StaffName ? row.StaffName : staffNm.StaffName*/;
                    row.Charge = row.Charge;
                    row.Total = row.Total;
                    row.ExpiringDate = row.ExpiringDate;
                    row.RAM = row.RAM;
                    row.HardDisk = row.HardDisk;
                    row.HDD_Used = row.HDD_Used;
                    row.HDD_Available = row.HDD_Available;
                    row.Access_Details = row.Access_Details;
                
                    /*== staffNm.StaffNo ? row.StaffNo : staffNm.StaffNo*/
                    ;

                    if (servNm == null)
                    {
                        var entity = row.Create(row);
                        _db.Add(entity);
                    }
                    else
                    {
                        var entity = row.Edit(servNm, row);
                        _db.Update(entity);
                    }
                    var result = _db.SaveChanges();
                    if (result.Succeeded == false)
                    {
                        row.Message = result.Message;
                        errors.Add(row);
                    }
                }
                return errors;
            }));


        }
    }
}