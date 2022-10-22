using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Covid19MVC.Models;

namespace Covid19MVC.Controllers
{
    public class PersonalInfoesController : Controller
    {
        private covid19DBEntities db = new covid19DBEntities();

        // GET: PersonalInfoes
        public ActionResult Index()
        {
            return View(db.PersonalInfo.ToList());
        }

        // GET: PersonalInfoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonalInfo personalInfo = db.PersonalInfo.Find(id);
            if (personalInfo == null)
            {
                return HttpNotFound();
            }
            return View(personalInfo);
        }

        // GET: PersonalInfoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PersonalInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,City,Street,NumberStreet,DateOfBirth,Phone,Cellphone,VacDate1,VacDate2,VacDate3,VacDate4,VacMaker1,VacMaker2,VacMaker3,VacMaker4,SickDate,RecoveryDate")] PersonalInfo personalInfo)
        {
            if (ModelState.IsValid)
            {
                PersonalInfo personalInfo1 = db.PersonalInfo.Find(personalInfo.Id);
                if (personalInfo1 != null)
                {
                    ViewBag.Message = "This ID exists";
                    return View(personalInfo);
                }

                if (personalInfo.Id.ToString().Length > 9)
                {
                    ViewBag.Message = "The ID length is incorrect";
                    return View(personalInfo);
                }

                if ((personalInfo.VacDate1.HasValue && personalInfo.VacMaker1 == null) ||
                    (personalInfo.VacDate1 == null && personalInfo.VacMaker1 != null) ||
                    (personalInfo.VacDate2.HasValue && personalInfo.VacMaker2 == null) ||
                    (personalInfo.VacDate2 == null && personalInfo.VacMaker2 != null) ||
                    (personalInfo.VacDate3.HasValue && personalInfo.VacMaker3 == null) ||
                    (personalInfo.VacDate3 == null && personalInfo.VacMaker3 != null) ||
                    (personalInfo.VacDate4.HasValue && personalInfo.VacMaker4 == null) ||
                    (personalInfo.VacDate4 == null && personalInfo.VacMaker4 != null))
                {
                    ViewBag.Message = "Missing vaccination date or vaccine manufacturer";
                    return View(personalInfo);
                }

                if (personalInfo.SickDate >= personalInfo.RecoveryDate)
                {
                    ViewBag.Message = "The recovery date is before or the same as the sick date";
                    return View(personalInfo);
                }

                if (personalInfo.SickDate == null && personalInfo.RecoveryDate.HasValue)
                {
                    ViewBag.Message = "It is not possible to have a recovery date without a sick date";
                    return View(personalInfo);
                }

                if (personalInfo.Phone != null)
                {
                    if (personalInfo.Phone.Length != 9)
                    {
                        ViewBag.Message = "The number of digits in the house phone is incorrect";
                        return View(personalInfo);
                    }
                    string Numbers = "0123456789";
                    for (int i = 0; i < personalInfo.Phone.Length; i++)
                    {
                        if (Numbers.Contains(personalInfo.Phone.ElementAt(i)))
                        {
                            continue;
                        }
                        else
                        {
                            ViewBag.Message = "A phone number can only contain numbers";
                            return View(personalInfo);
                        }
                    }
                }

                if (personalInfo.Cellphone != null)
                {
                    if (personalInfo.Cellphone.Length != 10)
                    {
                        ViewBag.Message = "The number of digits in the cellphone is incorrect";
                        return View(personalInfo);
                    }
                    string Numbers = "0123456789";
                    for (int i = 0; i < personalInfo.Cellphone.Length; i++)
                    {
                        if (Numbers.Contains(personalInfo.Cellphone.ElementAt(i)))
                        {
                            continue;
                        }
                        else
                        {
                            ViewBag.Message = "A phone number can only contain numbers";
                            return View(personalInfo);
                        }
                    }
                }

                    db.PersonalInfo.Add(personalInfo);
                    db.SaveChanges();
                    return RedirectToAction("Index");
            }
            return View(personalInfo);
        }

        // GET: PersonalInfoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonalInfo personalInfo = db.PersonalInfo.Find(id);
            if (personalInfo == null)
            {
                return HttpNotFound();
            }
            return View(personalInfo);
        }

        // POST: PersonalInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,City,Street,NumberStreet,DateOfBirth,Phone,Cellphone,VacDate1,VacDate2,VacDate3,VacDate4,VacMaker1,VacMaker2,VacMaker3,VacMaker4,SickDate,RecoveryDate")] PersonalInfo personalInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(personalInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(personalInfo);
        }

        // GET: PersonalInfoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonalInfo personalInfo = db.PersonalInfo.Find(id);
            if (personalInfo == null)
            {
                return HttpNotFound();
            }
            return View(personalInfo);
        }

        // POST: PersonalInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PersonalInfo personalInfo = db.PersonalInfo.Find(id);
            db.PersonalInfo.Remove(personalInfo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
