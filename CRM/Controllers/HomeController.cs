using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeekHunterDataSource.EF;
using CRM.Models;
using GeekHunterDataSource.UnitOfWork;
using GeekerHunterServices;
using GeekerHunterServices.Models;

namespace CRM.Controllers
{
    public class HomeController : Controller
    {
        private ICRMService crsService;
      
        // GET: Home
        public HomeController(ICRMService crsServices)
        {
           this.crsService = crsServices;
        }
        public ActionResult Index()
        {
            var work = new GeekHunterUnitOfWork(new GeekHunterEntities());
            ViewBag.skills = work.Skills.GetAll().ToList();
            return View();
        }
        public ActionResult List()
        {
            var returnMV = new CandidatesMV();
            var list = crsService.GetCandidateList();
            returnMV.CandidatesList = list;
            return View(returnMV);
        }
        public ActionResult ListChange(CandidatesMV returnMV)
        {
            return View("List", returnMV);
        }
        public ActionResult FilterList(string skillsString)
        {
            var list = crsService.GetCandidateListBySkill(skillsString);

            var returnMV = new CandidatesMV();
            returnMV.CandidatesList = list;

            return Json(returnMV);
        }
        
        public ActionResult AddNewCandidate()
        {
            var Db = new GeekHunterEntities();
            ViewBag.skills = Db.Skills.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult AddNewCandidate(CandidateMV candiate, int[] skills)
        {
           if (candiate.FirstName == null || candiate.LastName == null || skills.Length == 0)
            {
                string FailureMessage = "";
                if (candiate.FirstName == null)
                {
                    FailureMessage += "FirstName is empty ! ";
                }
                if (candiate.FirstName == null)
                {
                    FailureMessage += "Lastname is empty ! ";
                }
                if (candiate.FirstName == null)
                {
                    FailureMessage += "Skill is empty ! ";
                }
                TempData["message"] = "failure:"+FailureMessage;
                return RedirectToAction("Index");

            }
            else
            {
                var createNew = new CandidateModel()
                {
                    FirstName = candiate.FirstName,
                    LastName = candiate.LastName
                };
                var result = crsService.CreateNewCandidate(skills, createNew);
                TempData["message"] = "successfully added!";
                return RedirectToAction("Index");
            }
        }
    }
}
