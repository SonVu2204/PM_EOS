using System;
using System.Collections.Generic;

#nullable disable

namespace PM_EOS.Models
{
    public partial class Mark
    {
        public int Iddiem { get; set; }
        public int? HocSinhId { get; set; }
        public int? MonHocId { get; set; }
        public int? DeThiId { get; set; }
        public int? DiemThi { get; set; }
        public string Status { get; set; }

        public virtual DeThi DeThi { get; set; }
        public virtual Account HocSinh { get; set; }
        public virtual MonHoc MonHoc { get; set; }
    }
}
