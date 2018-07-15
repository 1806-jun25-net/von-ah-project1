using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.WebApp.Models
{
    public class OrderWeb
    {
        public int OrderId { get; set; }
        public int LocationId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderTime { get; set; }
        public decimal TotalPrice { get; set; }
        public List<int> PizzaIDs { get; set; } = new List<int>();
        //public List<string> PizzasString { get; set; }
        public int NumOfToppings { get; set; } = 2;
        public Dictionary<int, List<bool>> PizzaDictionary { get; set; } = new Dictionary<int, List<bool>>();
        public string PizzaCountString { get; set; }
        public int PizzaCountInt { get; set; }
        public string Address { get; set; }
        public bool[] CheckBoxValues { get; set; }
        public string UserName { get; set; }

        public List<SelectListItem> AddressEnumerable = new List<SelectListItem>
        {
            new SelectListItem {Value = "123 Grove St.", Text = "123 Grove St."},
            new SelectListItem {Value = "21 Jump St.", Text = "21 Jump St."},
            new SelectListItem {Value = "221B Baker St.", Text = "221B Baker St."}
        };


        public List<SelectListItem> NumOfPizzaEnumerable = new List<SelectListItem>
        {
            new SelectListItem {Value = "1", Text = "1"},
            new SelectListItem {Value = "2", Text = "2"},
            new SelectListItem {Value = "3", Text = "3"},
            new SelectListItem {Value = "4", Text = "4"},
            new SelectListItem {Value = "5", Text = "5"},
            new SelectListItem {Value = "6", Text = "6"},
            new SelectListItem {Value = "7", Text = "7"},
            new SelectListItem {Value = "8", Text = "8"},
            new SelectListItem {Value = "9", Text = "9"},
            new SelectListItem {Value = "10", Text = "10"},
            new SelectListItem {Value = "11", Text = "11"},
            new SelectListItem {Value = "12", Text = "12"}
        };
    }
}
