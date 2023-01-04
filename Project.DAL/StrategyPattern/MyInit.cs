using Project.COMMON.Tools;
using Project.DAL.ContextClasses;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.StrategyPattern
{
    
    public class MyInit:CreateDatabaseIfNotExists<MyContext>
    {
        protected override void Seed(MyContext context)
        {
            #region Admin

            AppUser au = new AppUser();
            au.UserName = "su";
            au.Password = DantexCrypt.Crypt("123");
            au.Email = "echo579@gmail.com ";
            au.Role = ENTITIES.Enums.UserRole.Admin;
            context.AppUsers.Add(au);
            context.SaveChanges();


            #endregion
           
            AppUser ap = new AppUser();
            ap.UserName = "mu";
            ap.Password = "123";
            ap.Email ="mgut15@gmail.com";
            ap.Active = true;
            context.AppUsers.Add(ap);                      
            context.SaveChanges();


            UserProfile up = new UserProfile();
            up.ID = 2;
            up.FirstName = "Mehtap";
            up.LastName = "Uğur";
            up.Address = "gqgqg";
            context.UserProfiles.Add(up);          
            context.SaveChanges();

            Category c = new Category();
            c.CategoryName = "Bilim";
            c.Description = "Gerçek";


            Product p = new Product();
            p.ProductName = "Kitap" ;                 
            p.UnitPrice = 18 ;
            p.UnitsInStock = 150;
            //b.ImagePath = 
            c.Products.Add(p);
              
            context.Categories.Add(c);
            context.SaveChanges();

            
        }
    }
}
