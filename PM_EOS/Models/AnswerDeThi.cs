using System;
using System.Collections.Generic;

#nullable disable

namespace PM_EOS.Models
{
    public partial class AnswerDeThi
    {
        public int? DeThiId { get; set; }
        public int? CauHoiId { get; set; }
        public int? DapAnId { get; set; }

        public virtual CauHoi CauHoi { get; set; }
        public virtual DapAn DapAn { get; set; }
        public virtual DeThi DeThi { get; set; }
    }
}
