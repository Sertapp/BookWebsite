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
    public class RegisterController : Controller
    {
        AppUserRepository _apRep;
        ProfileRepository _proRep;

        public RegisterController()
        {
            _apRep = new AppUserRepository();
            _proRep = new ProfileRepository();
        }

        // GET: Register
        public ActionResult RegisterNow()
        {
            return View();
        }

        [HttpPost]        
        public ActionResult RegisterNow(AppUserVM apvm)
        {
            AppUser appUser = apvm.AppUser;
            UserProfile profile = apvm.Profile;

            appUser.Password = DantexCrypt.Crypt(appUser.Password);

            if (_apRep.Any(x=>x.UserName==appUser.UserName))
            {
                ViewBag.OncedenKayitli = "Kullanıcı adı önceden alınmıstır.";
                return View();
            }
            else if (_apRep.Any(x=>x.Email == appUser.Email))
            {
                ViewBag.OncedenKayitli = "Email önceden kaydedilmiştir.";
                return View();
            }

            //Kullanıcı register islemini tamamlarsa mail gelir.

            string gidecekMail = "Hesabınız olusmustur.Hesabınızı aktive etmek için https://localhost:44358/Register/Activation/" + appUser.ActivationCode + "linke tıklayınız.";

            MailService.Send(appUser.Email, body: gidecekMail, subject:"Hesap aktivasyonu yapılmalı...");
            _apRep.Add(appUser);
            //Once bu eklenmeli ki AppUser'in ID si ilk basta gelmelidir.Cunku burada AppUser zorunlu, Profile istege bağlı bir alandır.ID'yi SaveChanges(veri kaynagındaki güncelleştirmeleri devam ettirmek) ile olustururuz ki Profile'ı olustururuz.

            //Post tarafında instance alarak gonderildiginden ismini ve soyismini girmek istemiyorsa
            if (!string.IsNullOrEmpty(profile.FirstName) || !string.IsNullOrEmpty(profile.LastName) || !string.IsNullOrEmpty(profile.Address))
            {
                profile.ID = appUser.ID;
                _proRep.Add(profile);
            }

            return View("RegisterOk");
        }

        public ActionResult Activation(Guid id)
        {
            AppUser aktifHaleGetirilecek = _apRep.FirstOrDefault(x => x.ActivationCode == id);
            if (aktifHaleGetirilecek != null)
            {
                aktifHaleGetirilecek.Active = true;
                _apRep.Update(aktifHaleGetirilecek);
                TempData["HesapAktiflestirildiMi"] = "Hesap aktif hale getirildi.";
                return RedirectToAction("Login","Home");
            }
            TempData["HesapAktiflestirildiMi"] = "Hesap bulunamadı.";
            return RedirectToAction("Login", "Home");
        }

        public ActionResult RegisterOk()
        {
            return View();
        }
    }
}