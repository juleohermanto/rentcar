using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentCar.Models
{
    [Table("TrRental")]
    public class TrRental
    {
        [Key]
        public string Rental_id { get; set; } 
        public DateTime rental_date { get; set; } 
        public DateTime return_date { get; set; }
        public decimal total_price { get; set; }
        public bool payment_status { get; set; }

        public string customer_id { get; set; } 
        public string car_id { get; set; } 

        [ForeignKey("car_id")]
        public MsCar MsCar { get; set; }
    }
}