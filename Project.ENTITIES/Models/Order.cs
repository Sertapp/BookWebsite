using Project.ENTITIES.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
    public class Order:BaseEntity
    {
        public string ShippedAddress { get; set; }
        public decimal TotalPrice { get; set; }
        public int? AppUserID { get; set; }

        //Siparis islemlerinin icerisindeki bilgileri daha rahat yakalamak için acılan property'lerdir.
        public string UserName { get; set; }
        public string Email { get; set; }

        public DeliveryStatus Delivery { get; set; }
        public Order()
        {
            Delivery = DeliveryStatus.Preparing;
        }

        //Relational Properties
        public virtual AppUser AppUser { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }

    }
}
