using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentCar.Models
{
    [Table("MsCustomer")] 
    public class MsCustomer
    {
        [Key] 
        public string Customer_id { get; set; } = Guid.NewGuid().ToString(); //Saya kebetulan menggunakan Id secara otomatis

        public string email { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string phone_number { get; set; }
        public string? address { get; set; } 
        public string? driver_license_number { get; set; }
    }
}