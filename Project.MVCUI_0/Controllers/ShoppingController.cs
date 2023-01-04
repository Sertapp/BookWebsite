using PagedList;
using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.COMMON.Tools;
using Project.ENTITIES.Models;
using Project.MVCUI_0.Models.ShoppingTools;
using Project.MVCUI_0.VMClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI_0.Controllers
{
    public class ShoppingController : Controller
    {
        OrderRepository _oRep;
        ProductRepository _pRep;
        CategoryRepository _cRep;
        OrderDetailRepository _odRep;
       

        public ShoppingController()
        {
            _oRep = new OrderRepository();
            _pRep = new ProductRepository();
            _cRep = new CategoryRepository();
            _odRep = new OrderDetailRepository();
        }

        // GET: Shopping
        public ActionResult ShoppingList(int? page, int? categoryID)
        {
             PAVM pavm = new PAVM
             {
               PagedProducts = categoryID == null ? _pRep.GetActives().ToPagedList(page ?? 1, 9) : _pRep.Where(x => x.CategoryID == categoryID).ToPagedList(page ?? 1, 9),
                Categories = _cRep.GetActives()
             };
             if (categoryID != null) TempData["catID"] = categoryID;
             return View(pavm);
        }
        public ActionResult AddToCart(int id)
        {
             Cart c = Session["scart"] == null ? new Cart() : Session["scart"] as Cart;

             Product eklenecekUrun = _pRep.Find(id);
             CartItem ci = new CartItem
             {
                  ID = eklenecekUrun.ID,
                  Name = eklenecekUrun.ProductName,
                  Price = eklenecekUrun.UnitPrice,
                  ImagePath = eklenecekUrun.ImagePath
             };
             c.SepeteEkle(ci);
             Session["scart"] = c;
             return RedirectToAction("ShoppingList");

        }
        public ActionResult CartPage()
        {
            if (Session["scart"]!= null)
            {
                CartPageVM cpvm = new CartPageVM();
                Cart c = Session["scart"] as Cart;
                cpvm.Cart = c;
                return View(cpvm);
            }
            TempData["sepetBos"] = "Sepetinizde ürün bulunmamaktadir";
            return RedirectToAction("ShoppingList");
        }

        public ActionResult DeleteFromCart(int id)
        {
            if (Session["scart"] !=null)
            {
                Cart c = Session["scart"] as Cart;
                c.SepettenSil(id);
                if (c.Sepetim.Count == 0)
                {
                    Session.Remove("scart");
                    TempData["sepetBos"] = "Sepetinizde ürün bulunmamaktadır";
                    return RedirectToAction("ShoppingList");
                }
                return RedirectToAction("CartPage");
            }
            return RedirectToAction("ShoppingList");
        }

        public ActionResult SiparisiOnayla()
        {
            AppUser mevcutKullanici;
            if (Session["memmber"] != null)
            {
                mevcutKullanici = Session["member"] as AppUser;
                return View();
            }
            else
            {
                TempData["anonim"] = "Üye olunuz";
                return RedirectToAction("RegisterNow", "Register");
            }

        }

        //https://localhost:44331/api/Payment/ReceivePayment
        [HttpPost]
        public ActionResult SiparisiOnayla(OrderVM ovm)
        {
            bool result;
            Cart sepet = Session["scart"] as Cart;
            ovm.Order.TotalPrice = ovm.PaymentDTO.ShoppingPrice = sepet.TotalPrice;
            
           using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44331/api/");
                Task<HttpResponseMessage> posTask = client.PostAsJsonAsync("Payment/ReceivePayment", ovm.PaymentDTO);
                HttpResponseMessage sonuc;

                try
                {
                    sonuc = posTask.Result;
                }
                catch (Exception ex)
                {

                    TempData["baglantiRed"] = "Banka baglantıyı reddetti";
                    return RedirectToAction("ShoppingLİst");
                }

                if (sonuc.IsSuccessStatusCode)
                {
                    result = true;
                }
                else result = false;

                if (result)
                {
                    if (Session["member"] != null)
                    {
                        AppUser kullanici = Session["member"] as AppUser;
                        ovm.Order.AppUserID = kullanici.ID;
                        ovm.Order.UserName = kullanici.UserName;
                    }

                    _oRep.Add(ovm.Order);
                    foreach (CartItem item in sepet.Sepetim)
                    {
                        OrderDetail od = new OrderDetail();
                        od.OrderID = ovm.Order.ID;
                        od.ProductID = item.ID;
                        od.TotalPrice = item.SubTotal;
                        od.Quantity = item.Amount;
                        _odRep.Add(od);

                        Product stokDus = _pRep.Find(item.ID);
                        stokDus.UnitsInStock -= item.Amount;
                        _pRep.Update(stokDus);
                    }
                    Session["scart"] = null;
                    TempData["odeme"] = "Siparişiniz bize ulaşmıştır.Teşekkürler";
                    MailService.Send(ovm.Order.Email, body: $"Siparişiniz alınmıştır. {ovm.Order.TotalPrice}", subject:"Sipariş Detayı");
                    return RedirectToAction("HomePage","MainPage");
                }

                else
                {
                    TempData["sorun"] = "Odeme ile ilgili sorun olusmustur, lütfen bankanız ile iletişime geçiniz";
                    return RedirectToAction("ShoppingList");
                }
            }









            
        }
        
    }
}