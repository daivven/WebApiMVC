using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApiMVC.Models;
using WebApiMVC.ViewModel;
using Newtonsoft.Json;

namespace WebApiMVC.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer  
        public ActionResult Index()
        {
            CustomerClient CC = new CustomerClient();
            ViewBag.listCustomers = CC.findAll();

            return View();
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View("Create");
        }
        [HttpPost]
        public ActionResult Create(CustomerViewModel cvm)
        {
            CustomerClient CC = new CustomerClient();
            CC.Create(cvm.customer);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            CustomerClient CC = new CustomerClient();
            CC.Delete(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            CustomerClient CC = new CustomerClient();
            CustomerViewModel CVM = new CustomerViewModel();
            CVM.customer = CC.find(id);
            return View("Edit", CVM);
        }
        [HttpPost]
        public ActionResult Edit(CustomerViewModel CVM)
        {
            CustomerClient CC = new CustomerClient();
            CC.Edit(CVM.customer);
            return RedirectToAction("Index");
        }
    }
}