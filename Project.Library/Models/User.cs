using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Library.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DefaultAddress { get; set; }
        public bool ManagerFlag { get; set; }
    }
}
