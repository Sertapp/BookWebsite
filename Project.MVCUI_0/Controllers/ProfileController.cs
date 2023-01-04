using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.COMMON.Tools;
using Project.ENTITIES.Models;
using Project.MVCUI_0.VMClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Project.MVCUI_0.Controllers
{
    public class ProfileController : Controller
    {
        AppUserRepository _apRep;

        // GET: Profile
        public ProfileController()
        {
            _apRep = new AppUserRepository();
        }

        public ActionResult ProfilePage()
        {
            if (Session["member"] == null)
            {
                TempData["msg"] = "<script>alert('Giriş yapınız');</script>";
                return RedirectToAction("Login", "Home");
            }


            AppUser user = Session["member"] as AppUser;

            AppUserVM apvm = new AppUserVM()
            {
                AppUser = user
            };
            return View(apvm);
        }
        public ActionResult ProfileEdit(int id)
        {
            AppUserVM apvm = new AppUserVM();
            apvm.AppUser = _apRep.FirstOrDefault(x => x.ID == id);           
            apvm.AppUser.Password = DantexCrypt.DeCrypt(apvm.AppUser.Password);
            return View(apvm);
        }
        [HttpPost]
        public ActionResult ProfileEdit(AppUser appUser)
        {
            AppUser update = _apRep.FirstOrDefault(x => x.ID == appUser.ID);
            if (update.Password == appUser.ConfirmPassword)
            {
                update.Password = update.ConfirmPassword = appUser.ConfirmPassword = appUser.Password = DantexCrypt.Crypt(appUser.Password);

                update.Profile.FirstName = appUser.Profile.FirstName;
                update.Profile.LastName = appUser.Profile.LastName;
                update.Profile.Address = appUser.Profile.Address;

                _apRep.Update(update);
                return RedirectToAction("ProfilePage");
            }
            TempData["sifre"] = "Şifre yanlış";
            return RedirectToAction("ProfileEdit");
        }
        public ActionResult MailSend()
        {

            return View();
        }

        [HttpPost]
        public ActionResult MailSend(AppUserVM apvm)
        {
            AppUser mail = _apRep.FirstOrDefault(x => x.Email == apvm.AppUser.Email);
            if (mail != null)
            {
                string sendMail = "Hesabını silmek için yandaki linke tıklayınız :(. https://localhost:44358/Profile/DestroyUser/" + mail.ActivationCode;
                MailService.Send(mail.Email, body: sendMail, subject: "Hesap Silme");
                ViewBag.info = "Hesap silme işlemi mailinize gönderilmiştir";
                return View();
            }
            ViewBag.info = "Mail bulunamadı";
            return View();
        }
        public ActionResult DestroyUser(Guid id)
        {

            AppUser destroy = _apRep.FirstOrDefault(x => x.ActivationCode == id);
            if (destroy != null)
            {
                _apRep.Destroy(destroy);
            }
           return RedirectToAction("HomePage", "MainPage");
        }


    }
}