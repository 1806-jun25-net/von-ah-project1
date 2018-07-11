using System;
using System.Collections.Generic;

namespace Project1.Context.Models
{
    public partial class Users
    {
        public Users()
        {
            Orders = new HashSet<Orders>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DefaultAddress { get; set; }
        public bool? ManagerFlag { get; set; }

        public ICollection<Orders> Orders { get; set; }
    }
}
