using System;
using System.Collections.Generic;

#nullable disable

namespace PM_EOS.Models
{
    public partial class MonHocDeThi
    {
        public int MonHocId { get; set; }
        public int DeThiId { get; set; }
        
        public int Khoa_Chinh { get; set; }
        public virtual DeThi DeThi { get; set; }
        public virtual MonHoc MonHoc { get; set; }
    }
}
