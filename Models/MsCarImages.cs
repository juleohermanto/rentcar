using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentCar.Models
{
    public class MsCarImages
    {
        [Key]
        public string image_car_id { get; set; }
        public string Car_id { get; set; }

        public string image_link { get; set; }

        [ForeignKey("Car_id")]
        public MsCar MsCar { get; set; }
    }
}