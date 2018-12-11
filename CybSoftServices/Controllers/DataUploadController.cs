using CybSoftServices.Interface;
using CybSoftServices.Interface.Utils;
using CybSoftServices.Models;
using Microsoft.AspNet.Identity;
using nVisionIT.ExcelProcessor.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CybSoftServices.Controllers
{
    public class DataUploadController : Controller
    {
        private IUploadManager _uploadMgr;
        private IServiceManager _servMgr;
        private IExcelProcessor _excel;
        private ApplicationUserManager _userManager;
        public DataUploadController(ApplicationUserManager userManager, IUploadManager uploadMgr, IServiceManager servMgr, IExcelProcessor excel)
        {
            _uploadMgr = uploadMgr;
            _servMgr = servMgr;
            _excel = excel;
            _userManager = userManager;

        }
        // GET: DataUpload
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult ServiceNames()
        {
            CheckTempData();
            DropDown();
            var model = new ServiceModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult ServiceNames(HttpPostedFileBase file, ServiceModel model)
        {
            DropDown();
            if (file != null && file.ContentLength != 0 && (System.IO.Path.GetExtension(file.FileName).ToLower() == ".xlsx" || System.IO.Path.GetExtension(file.FileName).ToLower() == ".xls"))
            {
                model.CreatedBy = User.Identity.GetUserName();
                model.ModifiedBy = User.Identity.GetUserName();
                var uploadedResult = _uploadMgr.UploadServiceNames(file.InputStream, model);

                if (uploadedResult.Succeeded == false)
                {
                    ViewBag.Error = $"{uploadedResult.Message}";
                    ModelState.AddModelError(string.Empty, uploadedResult.Message);
                    return View(model);
                }
                //TempData["message"] = new ResponseModel { Success = $" Names was successfully Uploaded!", Error = "" };
                //return RedirectToAction("AdmittedStudents");
            }
            ModelState.AddModelError(string.Empty, "Only Excel Sheets are allowed");
            return View(model);
        }
        private void DropDown()
        {
            ViewBag.voters = new SelectList(_servMgr.GetServices().Result, "VoterId", "StaffName");

        }
        private void CheckTempData()
        {
            if (TempData["message"] != null)
            {
                var responseModel = (ResponseModel)TempData["message"];
                ViewBag.Success = !string.IsNullOrEmpty(responseModel.Success) ? responseModel.Success : "";
                ViewBag.Error = !string.IsNullOrEmpty(responseModel.Error) ? responseModel.Error : "";
                ModelState.AddModelError(string.Empty, !string.IsNullOrEmpty(responseModel.Success) ? responseModel.Success : responseModel.Error);
            }
            TempData["message"] = null;
        }
        public ActionResult DownloadServiceNameTemplate()
        {
            return Redirect("~/DataUploadTemplates/ServiceNamesample.xlsx");
        }

        [HttpGet]
        public ActionResult ServerNames()
        {
            //CheckTempData();
            //DropDown();
            var model = new ServerModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult ServerNames(HttpPostedFileBase file, ServerModel model)
        {
            //DropDown();
            if (file != null && file.ContentLength != 0 && (System.IO.Path.GetExtension(file.FileName).ToLower() == ".xlsx" || System.IO.Path.GetExtension(file.FileName).ToLower() == ".xls"))
            {
                model.CreatedBy = User.Identity.GetUserName();
                model.ModifiedBy = User.Identity.GetUserName();
                var uploadedResult = _uploadMgr.UploadServerNames(file.InputStream, model);

                if (uploadedResult.Succeeded == false)
                {
                    ViewBag.Error = $"{uploadedResult.Message}";
                    ModelState.AddModelError(string.Empty, uploadedResult.Message);
                    return View(model);
                }
                //TempData["message"] = new ResponseModel { Success = $" Names was successfully Uploaded!", Error = "" };
                //return RedirectToAction("AdmittedStudents");
            }
            ModelState.AddModelError(string.Empty, "Only Excel Sheets are allowed");
            return View(model);
        }
        //private void DropDown()
        //{
        //    ViewBag.voters = new SelectList(_servMgr.GetServices().Result, "VoterId", "StaffName");

        //}
        //private void CheckTempData()
        //{
        //    if (TempData["message"] != null)
        //    {
        //        var responseModel = (ResponseModel)TempData["message"];
        //        ViewBag.Success = !string.IsNullOrEmpty(responseModel.Success) ? responseModel.Success : "";
        //        ViewBag.Error = !string.IsNullOrEmpty(responseModel.Error) ? responseModel.Error : "";
        //        ModelState.AddModelError(string.Empty, !string.IsNullOrEmpty(responseModel.Success) ? responseModel.Success : responseModel.Error);
        //    }
        //    TempData["message"] = null;
        //}
        public ActionResult DownloadServerNameTemplate()
        {
            return Redirect("~/DataUploadTemplates/ServerNamesample.xlsx");
        }

    }
}