using CybSoftServices.Interface;
using CybSoftServices.Models;
using Microsoft.AspNet.Identity;
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
        public ServiceController(IServiceManager servMgr)
        {
            _servMgr = servMgr;
        }

        // GET: Service
        public ActionResult Index()
        {
            if (TempData["message"] != null)
            {
                ViewBag.Success = (string)TempData["message"];
            }
            var results = _servMgr.GetServices();
            if (results.Succeeded)
            {
                return View(results.Unwrap());
            }

            else
            {
                ModelState.AddModelError(string.Empty, "An error occure");
                return View();
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

            //var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors })
            //    .ToArray();

            if (ModelState.IsValid)
            {
                try
                {
                    model.CreatedBy = User.Identity.GetUserName();
                    var result = _servMgr.CreateService(model);
                    if (result.Succeeded == true)
                    {
                        TempData["message"] = $"Service{model.Name} was successful added";
                        return RedirectToAction("Index");
                    }
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
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

                    TempData["message"] = $"Service{model.Name} was successfully added!";
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
    }
}