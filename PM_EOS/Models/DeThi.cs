using System;
using System.Collections.Generic;

#nullable disable

namespace PM_EOS.Models
{
    public partial class DeThi
    {
        public DeThi()
        {
            Marks = new HashSet<Mark>();
        }

        public int IddeThi { get; set; }
        public string TenDeThi { get; set; }
        public string TrangThai { get; set; }

        public virtual ICollection<Mark> Marks { get; set; }
    }
}
