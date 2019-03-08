using Demo.Infrastructure;
using Demo.Models;
using System;
using System.Net.Http;
using System.Web.Mvc;

namespace Demo.Controllers
{
    public class HomeController : Controller
    {
        private ICompanyRepository _repo = null;

        public HomeController(ICompanyRepository repo)
        {
            _repo = repo;
        }

        public ActionResult Index()
        {            
            return View(_repo.GetAll());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create ([Bind(Exclude = "Id")] Company model)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = _repo.AddCompany(model);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");
                else
                    ModelState.AddModelError(string.Empty, response.ReasonPhrase);                
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            Company company = _repo.GetCompanyById(id);
            if (null != company)
                return View(company);
            else
                return View("Error", new ApplicationException(string.Format(Constants.MESSAGE_ERROR_NO_ID, id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Company model)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage result = _repo.UpdateCompany(model);
                if (result.IsSuccessStatusCode)
                    return RedirectToAction("Index");
                else
                    ModelState.AddModelError(string.Empty, result.ReasonPhrase);               
            }

            return View(model);
        }

        public ActionResult Details(int id)
        {
            Company company = _repo.GetCompanyById(id);
            if (null != company)
                return View(company);
            else
                return View("Error", new ApplicationException(string.Format(Constants.MESSAGE_ERROR_NO_ID, id)));
        }

        public ActionResult Delete (int id)
        {
            HttpResponseMessage result = _repo.DeleteCompany(id);                
            if (result.IsSuccessStatusCode)
                return RedirectToAction("Index");                
            else
                return View("Error", new ApplicationException(result.ReasonPhrase));
        }
    }
}
