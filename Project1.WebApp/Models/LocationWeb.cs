using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.WebApp.Models
{
    public class LocationWeb
    {

        public int Id { get; set; }
        public string Address { get; set; }
        public Dictionary<string, int> Inventory { get; set; }
        public string SearchOption { get; set; }

        public List<SelectListItem> AddressEnumerable = new List<SelectListItem>
        {
            new SelectListItem {Value = "123 Grove St.", Text = "123 Grove St."},
            new SelectListItem {Value = "21 Jump St.", Text = "21 Jump St."},
            new SelectListItem {Value = "221B Baker St.", Text = "221B Baker St."}
        };


        public List<SelectListItem> OptionEnumerable = new List<SelectListItem>
        {
            new SelectListItem {Value = "1", Text = "Most Expensive"},
            new SelectListItem {Value = "2", Text = "Least Expensive"},
            new SelectListItem {Value = "3", Text = "Most Recent"},
            new SelectListItem {Value = "4", Text = "Least Recent"}
        };
    }
}
