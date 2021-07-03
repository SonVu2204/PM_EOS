using System;
using System.Collections.Generic;

#nullable disable

namespace PM_EOS.Models
{
    public partial class Account
    {
        public Account()
        {
            Marks = new HashSet<Mark>();
        }

        public int Idacc { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public virtual ICollection<Mark> Marks { get; set; }
    }
}
