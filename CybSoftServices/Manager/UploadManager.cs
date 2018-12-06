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
                    row.Name = model.Name;
                    row.Description = model.Description;
                    row.Email = model.Email;
                    row.ExpiredDate = row.ExpiredDate /*== staffNm.StaffName ? row.StaffName : staffNm.StaffName*/;
                    row.RenewerType = row.RenewerType;
                    row.CountDown = row.CountDown  /*== staffNm.StaffNo ? row.StaffNo : staffNm.StaffNo*/;
               
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