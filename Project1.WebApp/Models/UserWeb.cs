using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.WebApp.Models
{
    public class UserWeb
    {
        public int Id { get; set; }
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }
        [Display(Name = "Choose your location")]
        [Required]
        public string DefaultAddress { get; set; }

        [Display(Name = "Search order history by")]
        public int SearchOption { get; set;}

        public List<SelectListItem> OptionEnumerable = new List<SelectListItem>
        {
            new SelectListItem {Value = "1", Text = "Most Expensive"},
            new SelectListItem {Value = "2", Text = "Least Expensive"},
            new SelectListItem {Value = "3", Text = "Most Recent"},
            new SelectListItem {Value = "4", Text = "Least Recent"}
        };

        
        public List<SelectListItem> AddressEnumerable = new List<SelectListItem>
        {
            new SelectListItem {Value = "123 Grove St.", Text = "123 Grove St."},
            new SelectListItem {Value = "21 Jump St.", Text = "21 Jump St."},
            new SelectListItem {Value = "221B Baker St.", Text = "221B Baker St."}
        };
 

    }
}
