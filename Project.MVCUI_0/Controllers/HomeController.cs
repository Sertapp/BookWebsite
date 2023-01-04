using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.COMMON.Tools;
using Project.ENTITIES.Models;
using Project.MVCUI_0.VMClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI_0.Controllers
{
    public class HomeController : Controller
    {
        AppUserRepository _apRep;

        public HomeController()
        {
            _apRep = new AppUserRepository();
        }


        // GET: Home
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AppUser appUser)
        {
            AppUser esUser = _apRep.FirstOrDefault(x => x.UserName == appUser.UserName);

            if (esUser==null)
            {
                    ViewBag.Kullanici = "lütfen kullanıcı adınızı doğru giriniz";
                return View();
            }
           
            string decrypted = DantexCrypt.DeCrypt(esUser.Password);
            if (appUser.Password == decrypted && esUser.Role == ENTITIES.Enums.UserRole.Admin)
            {
                if (!esUser.Active)
                {
                    return AktifKontrol();
                }
                Session["admin"] = esUser;
                return RedirectToAction("CategoryList", "Category", new { Area = "Admin" });
            }
            else if (appUser.Password == decrypted && esUser.Role == ENTITIES.Enums.UserRole.Member)
            {
                if (!esUser.Active)
                {
                    return AktifKontrol();
                }
                TempData["eslesen"] = esUser.UserName;
                Session["member"] = esUser;
                return RedirectToAction("HomePage", "MainPage");
            }

            ViewBag.Kullanici = "Kullanıcı bulunamadı.";
            return View();
        }

        private ActionResult AktifKontrol()
        {
            ViewBag.AktifDegil = "Hesabınızı aktif hale getiriniz.Mailinizi kontrol ediniz.";
            return View("Login");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("HomePage", "MainPage");
        }

        public ActionResult ResetPasswordMailSend()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPasswordMailSend(AppUserVM apvm)
        {
            AppUser mail = _apRep.FirstOrDefault(x => x.Email == apvm.AppUser.Email);
            if (mail != null)
            {
                string sendMail = "Şifre sıfırlamak için: https://localhost:44358/Home/ResetPassword/" + mail.ActivationCode;
                MailService.Send(mail.Email, body: sendMail, subject: "Şİfre Değiştirme");
                ViewBag.info = "Şifre sfırlama isteğiniz mail adresinize gönderilmiştir";
                return View();
            }
            ViewBag.info = "Mail adresiniz bulunamadı";
            return View();
        }

        public ActionResult ResetPassword(Guid id)
        {
            AppUserVM apvm = new AppUserVM();
            apvm.AppUser = _apRep.FirstOrDefault(x => x.ActivationCode == id);
            return View(apvm);
        }

        [HttpPost]
         public ActionResult ResetPassword(AppUser appUser)
         {
            AppUser changePassword = _apRep.FirstOrDefault(x => x.ActivationCode == appUser.ActivationCode);
            changePassword.Password = DantexCrypt.Crypt(appUser.Password);

            changePassword.ConfirmPassword = DantexCrypt.Crypt(appUser.ConfirmPassword);
            _apRep.Update(changePassword);
            TempData["msg"] = "<script>alert('Şİfreniz degistirilmiştir');</script>";
            return RedirectToAction("Login", "Home");

        }
    }
}