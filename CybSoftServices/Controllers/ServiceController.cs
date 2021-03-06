﻿using CybSoftServices.Interface;
using CybSoftServices.Interface.Utils;
using CybSoftServices.Models;
using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CybSoftServices.Controllers
{
    [Authorize]
    public class ServiceController : Controller
    {
        private IServiceManager _servMgr;
        private IExcelProcessor _excel;
        public ServiceController(IServiceManager servMgr, IExcelProcessor excel)
        {
            _servMgr = servMgr;
            _excel = excel;
        }

        // GET: Service
        public ActionResult Index(int? page)
        {
            if (TempData["message"] != null)
            {
                ViewBag.Success = (string)TempData["message"];
            }
            var results = _servMgr.GetServices();
            if (results.Succeeded)
            {
                return View(results.Unwrap().ToPagedList(page ?? 1, 20));
                //return View(results.Unwrap());
            }

            else
            {
                ModelState.AddModelError(string.Empty, "An error occure");
               return Json(new { status = false, error = "Invalid Id" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult CreateService()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateService(ServiceModel model)
        {

            var serviceModel = new Operation<ServiceModel>();

            if (ModelState.IsValid)
            {
                try
                {
                    model.CreatedBy = User.Identity.GetUserName();
                    var result = _servMgr.CreateService(model);
                    serviceModel.Succeeded = true;

                    if (serviceModel.Succeeded == true)
                    {
                        TempData["message"] = $"Service{model.ServerDescription} was successful added";
                        return RedirectToAction("Index");
                    }
                }
                catch(Exception ex)
                {
                    throw ex;
                }
         
              
        }
            return View(model);
        }
        [HttpGet]
        public ActionResult EditService(int id = 0)
        {
            var result = _servMgr.GetServiceById(id);
            if (result.Succeeded)
            {
                return View(result.Result);
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View();
            }
        }
        [HttpPost]
        public ActionResult EditService(ServiceModel model)
        {
            if (ModelState.IsValid)
            {
                model.ModifiedBy = User.Identity.GetUserName();
                var result = _servMgr.UpdateService(model);
                if (result.Succeeded)
                {

                    TempData["message"] = $"Service{model.ServerDescription} was successfully added!";
                    ModelState.AddModelError(string.Empty, "Update was successfully ");
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View(model);

                }
            }
            return View(model);
        }

        public ActionResult Export()
        {
            var list = _servMgr.GetServices().Result;

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A1"].Value = "ServId";
            ws.Cells["B1"].Value = "ServerDescription";
            ws.Cells["C1"].Value = "Services";
            ws.Cells["D1"].Value = "ExpiredDate";
            ws.Cells["E1"].Value = "RenewerType";
            ws.Cells["F1"].Value = "Email ";
            ws.Cells["G1"].Value = "CountDown";
            ws.Cells["H1"].Value = "AlertExpired";
            ws.Cells["I1"].Value = "Access_Details";

            int rowStart = 2;
            foreach (var item in list)
            {
                //converting expireddate format to string 
                var stringDate = item.ExpiringDate;

                ws.Cells[string.Format("A{0}", rowStart)].Value = item.ServId;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.ServerDescription;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.Services;
                ws.Cells[string.Format("D{0}", rowStart)].Value = stringDate;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.RenewerType;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.Email;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.CountDown;
                ws.Cells[string.Format("H{0}", rowStart)].Value = item.AlertExpired;
                ws.Cells[string.Format("H{0}", rowStart)].Value = item.Access_Details;
                rowStart++;
            }

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();

            return RedirectToAction("Index");


        }

        [HttpPost]
        public JsonResult DeleteService(int id, string servName)
        {
            int votId = Convert.ToInt32(id);
            if (votId > 0)
            {
                var result = _servMgr.DeleteService(votId);
                if (result.Succeeded == true)
                {

                    return Json(new { status = true, message = $" {servName} has been successfully deleted!", JsonRequestBehavior.AllowGet });
                }
                return Json(new { status = false, error = result.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { status = false, error = "Invalid Id" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UploadServices()
        {
            DropDown();
            var model = new ServiceModel();
            return View(model);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UploadServices(HttpPostedFileBase file, ServiceModel model)
        {
            DropDown();
            try
            {
                if (file != null && file.ContentLength != 0 && (System.IO.Path.GetExtension(file.FileName).ToLower() == ".xlsx" || System.IO.Path.GetExtension(file.FileName).ToLower() == ".xls"))
                {
                    model.CreatedBy = User.Identity.GetUserName();
                    model.ModifiedBy = User.Identity.GetUserName();
                    var uploadedResult = _servMgr.UploadServiceNames(file.InputStream, model);
                    if (uploadedResult.Succeeded == true)
                    {

                        TempData["message"] = $"  successfully Uploaded!";
                        return RedirectToAction("Index");
                    }

                    //else
                    //{
                    //    ModelState.AddModelError(string.Empty, uploadedResult.Message);
                    //    ViewBag.Error = $"Error occured : {uploadedResult.Message}";
                    //    return View(model);
                    //}
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            //else
            //{
            //    ViewBag.Error = "Only Excel Sheets are allowed";
            //}
            //}
            return View();
        }
        private void DropDown()
        {
            ViewBag.voters = new SelectList(_servMgr.GetServices().Result, "ServId", "ServerDescription");
        }

        public ActionResult DownloadServiceTemplate()
        {
            //return Redirect("~/DataUploadTemplates/VoterNamesample.xls");
            return Redirect("~/DataUploadTemplates/ServiceNamesample.xlsx");

        }

    }
}