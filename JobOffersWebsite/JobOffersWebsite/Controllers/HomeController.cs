using System;
using System.Net;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using Job_Offers_Website.Models;
using JobOffersWebsite.Models;
using Microsoft.AspNet.Identity;

namespace JobOffersWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var list = db.Categories.ToList();
            return View(list);
        }

        public ActionResult Detailes(int JobId)
        {
            var job = db.Jobs.Find(JobId);

            if (job == null)
            {
                return HttpNotFound();
            }
            Session["JobId"] = JobId;
            
            return View(job);
        }


        [Authorize]
        public ActionResult Apply()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Apply(string Message )
        {
            var UserId = User.Identity.GetUserId();
            int JobId = (int)Session["JobId"];


            var check = db.ApplyForJobs.Where(a => a.JobId == JobId && a.UserId == UserId).ToList();

            if (check.Count() < 1)
            {
                var job = new ApplyForJob();

                job.UserId = UserId;
                job.JobId = JobId;
                job.Message = Message;
                job.ApplyDate = DateTime.Now;

                db.ApplyForJobs.Add(job);
                db.SaveChanges();
                ViewBag.Result = "تمت الاضافة بنجاح";
            }
            else
            {
                ViewBag.Result = "المعذرة لقد سبق وتقدمت الى نفس الوظيفة";
            }
            return View();
        }

        [Authorize]
        public ActionResult GetJobByUser()
        {
            var UserId = User.Identity.GetUserId();
            var Jobs = db.ApplyForJobs.Where(a => a.UserId == UserId);
            return View(Jobs.ToList());
        }

        [Authorize]
        public ActionResult DetailsOfJop(int Id)
        {
            var job = db.ApplyForJobs.Find(Id);

            if (job == null)
            {
                return HttpNotFound();
            }

            return View(job);
        }

        [Authorize]
        public ActionResult GetJobByPublisher()
        {
            var UserID = User.Identity.GetUserId();

            var Jobs = from app in db.ApplyForJobs
                       join Job in db.Jobs
                       on app.JobId equals Job.Id
                       where Job.User.Id == UserID
                       select app;

            var grouped = from j in Jobs
                          group j by j.Job.Title
                          into collection
                          select new JobsViewModel
                          {
                              JobTitle = collection.Key,
                              Items = collection
                          };
            return View(grouped.ToList());
        }

        public ActionResult Edit(int id)
        {
            var job = db.ApplyForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        public ActionResult Edit(ApplyForJob job)
        {
            if (ModelState.IsValid)
            {
                job.ApplyDate = DateTime.Now;
                db.Entry(job).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("GetJobByUser");
            }
            return View(job);
        }


        // GET: Roles/Delete/5
        public ActionResult Delete(int id)
        {
            var job = db.ApplyForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Roles/Delete/5
        [HttpPost]
        public ActionResult Delete(ApplyForJob job)
        {

            // TODO: Add delete logic here
            var myJob = db.ApplyForJobs.Find(job.Id);
            db.ApplyForJobs.Remove(myJob);
            db.SaveChanges();
            return RedirectToAction("GetJobByUser");

        }



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        [HttpPost]
        public ActionResult Contact(ContactModel contact)
        {
            var mail = new MailMessage();
            var loginInfo = new NetworkCredential("ahmedabdelatyeelu@gmail.com", "01208582007a");
            mail.From = new MailAddress(contact.Email);
            mail.To.Add(new MailAddress("ahmedabdelatyeelu@gmail.com"));
            mail.Subject = contact.Subject;
            mail.IsBodyHtml = true;
            string body = "أسم المرسل:" + contact.Name + "<br>" +
                        "بريد المرسل:" + contact.Email + "<br>" +
                        "عنوات الرسالة:" + contact.Subject + "<br>" +
                        "نص الرسالة:" + contact.Message;
            mail.Body = contact.Message;

            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(mail); 

            return RedirectToAction("Index");
        }
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string searchName)
        {
            var result = db.Jobs.Where(m => m.Title.Contains(searchName) ||
                        m.JobContent.Contains(searchName) ||
                        m.Category.CategoryName.Contains(searchName) ||
                        m.Category.CategoryDescription.Contains(searchName)).ToList();

                        return View(result);
        }
    }
}

