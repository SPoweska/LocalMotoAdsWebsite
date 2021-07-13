using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalMotoAdsWebsite.Models.ViewModels
{
    public class AdvertViewModel
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public string Descritpion { get; set; }
        public string VIN { get; set; }
        public string Year { get; set; }
        public string CarMileage { get; set; }
        public double Price { get; set; }
        public IFormFile ImagePath { get; set; }
        public Model Model { get; set; }
        public int ModelFK { get; set; }
    }
}
