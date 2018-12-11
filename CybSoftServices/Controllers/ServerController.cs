using CybSoftServices.Interface;
using CybSoftServices.Interface.Utils;
using CybSoftServices.Models;
using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CybSoftServices.Controllers
{
    public class ServerController : Controller
    {
        private IServerManager _servMgr;
        private IExcelProcessor _excel;
        public ServerController(IServerManager servMgr, IExcelProcessor excel)
        {
            _servMgr = servMgr;
            _excel = excel;
        }

        // GET: Service
        public ActionResult Index()
        {
            if (TempData["message"] != null)
            {
                ViewBag.Success = (string)TempData["message"];
            }
            var results = _servMgr.GetServers();
            if (results.Succeeded)
            {
                return View(results.Unwrap());
                //return View(results.Unwrap());
            }

            else
            {
                ModelState.AddModelError(string.Empty, "An error occure");
                return Json(new { status = false, error = "Invalid Id" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult CreateServer()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateServer(ServerModel model)
        {

            var serverModel = new Operation<ServerModel>();

            if (ModelState.IsValid)
            {
                try
                {
                    model.CreatedBy = User.Identity.GetUserName();
                    var result = _servMgr.CreateServer(model);
                    serverModel.Succeeded = true;

                    if (serverModel.Succeeded == true)
                    {
                        TempData["message"] = $"Server was successful added";
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }


            }
            return View(model);
        }
        [HttpGet]
        public ActionResult EditServer(int id = 0)
        {
            var result = _servMgr.GetServerById(id);
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
        public ActionResult EditServer(ServerModel model)
        {
            if (ModelState.IsValid)
            {
                model.ModifiedBy = User.Identity.GetUserName();
                var result = _servMgr.UpdateServer(model);
                if (result.Succeeded)
                {

                    TempData["message"] = $"Server was successfully added!";
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
            var list = _servMgr.GetServers().Result;

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A1"].Value = "ServerID";
            ws.Cells["B1"].Value = "Discription";
            ws.Cells["C1"].Value = "Services";
            ws.Cells["D1"].Value = "QTY";
            ws.Cells["E1"].Value = "Charge";
            ws.Cells["F1"].Value = "Total ";
            ws.Cells["G1"].Value = "ExpiringDate";
            ws.Cells["H1"].Value = "RAM";
            ws.Cells["I1"].Value = "HardDisk";
            ws.Cells["J1"].Value = "HDD_Used";
            ws.Cells["K1"].Value = "HDD_Available";
            ws.Cells["L1"].Value = "Access_Details";

            int rowStart = 2;
            foreach (var item in list)
            {
                //converting expireddate format to string 
                var stringDate = item.ExpiringDate;

                ws.Cells[string.Format("A{0}", rowStart)].Value = item.ServerID;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.Discription;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.Services;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.QTY;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.Charge;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.Total;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.ExpiringDate;
                ws.Cells[string.Format("H{0}", rowStart)].Value = item.RAM;
                ws.Cells[string.Format("I{0}", rowStart)].Value = item.HardDisk;
                ws.Cells[string.Format("J{0}", rowStart)].Value = item.HDD_Used;
                ws.Cells[string.Format("K{0}", rowStart)].Value = item.HDD_Available;
                ws.Cells[string.Format("L{0}", rowStart)].Value = item.Access_Details;
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
        public JsonResult DeleteServer(int id, string servName)
        {
            int votId = Convert.ToInt32(id);
            if (votId > 0)
            {
                var result = _servMgr.DeleteServer(votId);
                if (result.Succeeded == true)
                {

                    return Json(new { status = true, message = $" {servName} has been successfully deleted!", JsonRequestBehavior.AllowGet });
                }
                return Json(new { status = false, error = result.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { status = false, error = "Invalid Id" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UploadServers()
        {
            DropDown();
            var model = new ServerModel();
            return View(model);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UploadServers(HttpPostedFileBase file, ServerModel model)
        {
            DropDown();
            try
            {
                if (file != null && file.ContentLength != 0 && (System.IO.Path.GetExtension(file.FileName).ToLower() == ".xlsx" || System.IO.Path.GetExtension(file.FileName).ToLower() == ".xls"))
                {
                    model.CreatedBy = User.Identity.GetUserName();
                    model.ModifiedBy = User.Identity.GetUserName();
                    var uploadedResult = _servMgr.UploadServerNames(file.InputStream, model);
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
            ViewBag.voters = new SelectList(_servMgr.GetServers().Result, "ServerID", "Discription");
        }

        public ActionResult DownloadServerTemplate()
        {
            //return Redirect("~/DataUploadTemplates/VoterNamesample.xls");
            return Redirect("~/DataUploadTemplates/ServerNamesample.xlsx");

        }
    }
}