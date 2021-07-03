using PM_EOS.Models;
using PM_EOS.Mutiple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PM_EOS
{
    public class Caulenh
    {
        PM_EOSContext sd = new PM_EOSContext();
        // ham nay dung de lay ra  tai khoan dang nhap
        public Account GetAcc(string username, string pass)
        {
            Account acc = sd.Accounts.Where(x => x.Username == username && x.Password == pass).FirstOrDefault();
            return acc;
        }
        // lay ra list tat ca cac mon hoc
        public List<MonHoc> GetAllMonHoc()
        {
            List<MonHoc> list = sd.MonHocs.ToList();
            return list;
        }
        //lay ra de thi theo mon hoc
        public List<LKMonhocDeThi> GetAllDeThi(int id)
        {
            var linq = from x in sd.MonHocDeThis where (x.MonHocId == id)
                       join y in sd.DeThis on x.DeThiId equals y.IddeThi
                       select new LKMonhocDeThi { dethis = y, monhocdethis = x };
            List<LKMonhocDeThi> abc = linq.ToList();
            return abc;
        }
        //lay ra de thi dang bị khoa
        public List<DeThi> DeThiLock()
        {
            List<DeThi> abc = sd.DeThis.Where(x => x.TrangThai == "Lock").ToList();
            return abc;
        }
        // lay ra de thi dang duoc mo 
        public List<DeThi> DeThiUnLock()
        {
            List<DeThi> abc = sd.DeThis.Where(x => x.TrangThai == "UnLock").ToList();
            return abc;
        }
        // ham dung de sua trang thai de thi lock -> unlock
        public void ToUnLock(int id)
        {
            DeThi abc = sd.DeThis.Single(x => x.IddeThi == id);
            abc.TrangThai = "UnLock";
            sd.SaveChanges();
        }
        // ham dung de sua trang thai de thi unlock -> lock
        public void ToLock(int id)
        {
            DeThi abc = sd.DeThis.Single(x => x.IddeThi == id);
            abc.TrangThai = "Lock";
            sd.SaveChanges();
        }


        //chu y su ly phần lấy đề thi từ database
        // lay ra đe thi do co bao nhieu cau hoi 
        public  int SoLuongCauHoi(int id)
        {
            List<DetailDeThi> abc = sd.DetailDeThis.Where(x => x.DeThiId == id).ToList();
            int soluong = abc.Count();
            return soluong;
        }
        // lay ra theo  cac cau hoi
        public List<CauHoi_DetailDeThi_DapAn> GetDeThi(int id)
        {
            var linq = from x in sd.DetailDeThis
                       where (x.DeThiId == id)
                       join y in sd.CauHois on x.CauHoiId equals y.IdcauHoi
                       join z in sd.DapAns on x.DapAnId equals z.IddapAn
                       select new CauHoi_DetailDeThi_DapAn { dapans = z, detaildethis = x, cauhois = y };
            List<CauHoi_DetailDeThi_DapAn> abc = linq.ToList();
            return abc;
        }
        // lay ra 1 de co bao nhieu cau 
        public List<CauHoi> GetCauHoi1De(int id)
        {
            var linq = from x in sd.DetailDeThis where (x.DeThiId == id) 
                       join y in sd.CauHois on x.CauHoiId equals y.IdcauHoi
                       
                       select new CauHoi { IdcauHoi = y.IdcauHoi, NoiDungCauHoi = y.NoiDungCauHoi, TenCauHoi = y.TenCauHoi };
            List<CauHoi> abc = linq.Distinct().ToList();
            return abc;
        }
        /// <summary>
        /// lay ra dap an cua 1 de  thi 
        /// </summary>
        /// <param name="idde"></param>
        /// <returns></returns>
        public List<AnswerDeThi> DapAn1De(int idde)
        {
            List<AnswerDeThi> abc = sd.AnswerDeThis.Where(x => x.DeThiId == idde).ToList();
            return abc;
        }


    }
}
