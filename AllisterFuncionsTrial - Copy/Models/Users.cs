using System;
using System.Collections.Generic;
using System.Text;

namespace AllisterFuncionsTrial.Models
{
     public class Users
    {

        public string pk { get; set; }
        public string id { get; set; }
        public string username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateCreated { get; set; }

        public Users()
        {

            DateCreated = DateTime.Now;
        }


    }
}
