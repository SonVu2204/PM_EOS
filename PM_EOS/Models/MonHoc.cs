using System;
using System.Collections.Generic;

#nullable disable

namespace PM_EOS.Models
{
    public partial class MonHoc
    {
        public MonHoc()
        {
            Marks = new HashSet<Mark>();
        }

        public int IdmonHoc { get; set; }
        public string TenMonHoc { get; set; }

        public virtual ICollection<Mark> Marks { get; set; }
    }
}
