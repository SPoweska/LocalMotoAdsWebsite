using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalMotoAdsWebsite.Models.ViewModels
{
    public class ModelViewModel
    {
        public Model Model { get; set; }
        public IEnumerable<Make> Makes { get; set; }
        //public IEnumerable<SelectListItem> CSelectListItems(IEnumerable<Make> Items)
        //{
        //    List
        //}
    }
}
