using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LocalMotoAdsWebsite.Models
{
    public class Advert
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public string UserId { get; set; }
        public string Descritpion { get; set; }
        public string VIN { get; set; }
        public string Year { get; set; }
        public string CarMileage { get; set; }
        public double Price { get; set; }
        public string ImagePath { get; set; }
        public Model Model { get; set; }
        [ForeignKey("Model")]
        public int ModelFK { get; set; }
    }
}
