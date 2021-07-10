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
        /// <summary>
        /// Lay ra de thi theo mon hoc 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<LKMonhocDeThi> GetAllDeThi(int id)
        {
            var linq = from x in sd.MonHocDeThis where (x.MonHocId == id)
                       join y in sd.DeThis on x.DeThiId equals y.IddeThi
                       select new LKMonhocDeThi { dethis = y, monhocdethis = x };
            List<LKMonhocDeThi> abc = linq.ToList();
            return abc;
        }
        /// <summary>
        /// Lay ra đe thi dang bi khoa
        /// </summary>
        /// <returns></returns>
        public List<DeThi> DeThiLock()
        {
            List<DeThi> abc = sd.DeThis.Where(x => x.TrangThai == "Lock").ToList();
            return abc;
        }
        /// <summary>
        /// Lay ra de thi dang duoc mo
        /// </summary>
        /// <returns></returns>
        public List<DeThi> DeThiUnLock()
        {
            List<DeThi> abc = sd.DeThis.Where(x => x.TrangThai == "UnLock").ToList();
            return abc;
        }
        /// <summary>
        /// Lay ra tat ca cac de thi de sua 
        /// </summary>
        /// <returns></returns>
        public List<DeThi> GetAllDeThi()
        {
            List<DeThi> abc = sd.DeThis.ToList();
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
        /// <summary>
        /// dung de luu diem thi sau khi xong cua học sinh
        /// </summary>
        public void SaveDiemThi(int idhs, int monhoc,int dethi, int diemthi)
        {
            string kq ="";
            if(diemthi >= 5)
            {
                 kq = "Pass";
            }
            else
            {
                kq = "Not Pass";
            }
            Mark abc = new Mark()
            {
                HocSinhId = idhs,
                MonHocId = monhoc,
                DeThiId = dethi,
                DiemThi = diemthi,
                Status = kq
            };
            sd.Add(abc);
            sd.SaveChanges();
        }
        /// <summary>
        /// lay ra diem cua 1 hoc sinh
        /// </summary>
        /// <param name="idhs"></param>
        /// <returns></returns>
        public  List<MuitipleDiemThi> Diem1HocSinh( string idhs)
        {
            
           
            
                var linq = from x in sd.DeThis
                           join y in sd.Marks on x.IddeThi equals y.DeThiId 
                           where (y.HocSinhId.ToString() == idhs)
                           join z in sd.MonHocs on y.MonHocId equals z.IdmonHoc
                           select new MuitipleDiemThi { dethis = x, marks = y, monhocs = z };
            
          
            List<MuitipleDiemThi> abc = linq.ToList();
            return abc;
        }
        /// <summary>
        /// lay ra diem cu tat cả hoc sinh
        /// </summary>
        /// <param name="idhs"></param>
        /// <returns></returns>
        public List<MuitipleDiemThi> DiemAllHocSinh()
        {
           var linq = from x in sd.DeThis
                       join y in sd.Marks on x.IddeThi equals y.DeThiId
                       join z in sd.MonHocs on y.MonHocId equals z.IdmonHoc
                       join k in sd.Accounts on y.HocSinhId equals k.Idacc
                       select new MuitipleDiemThi { dethis = x, marks = y, monhocs = z ,accs=k};


            List<MuitipleDiemThi> abc = linq.ToList();
            return abc;
        }
        /// <summary>
        /// Dung de add de thi moi 
        /// </summary>
        public void AddDeThi( string  tende)
        {
            DeThi abc = new DeThi()
            {
                TenDeThi = tende,
                TrangThai = "Lock"
            };
            sd.Add(abc);
            sd.SaveChanges();
        }
        /// <summary>
        ///  dung de lay ra de thi vua add de add lien ket voi no
        /// </summary>
        /// <param name="tende"></param>
        /// <returns></returns>
        public DeThi Lay1DeThi(string tende)
        {
            DeThi abc = sd.DeThis.Where(x => x.TenDeThi == tende).FirstOrDefault();
            return abc;
        }
        /// <summary>
        ///  dung de add lien ket giua de thi vua tao voi mon hoc duoc chonn
        /// </summary>
        /// <param name="idmonhoc"></param>
        /// <param name="iddethi"></param>
        public void AddLienKetMonDe(int idmonhoc, int iddethi)
        {
            MonHocDeThi abc = new MonHocDeThi()
            {
                MonHocId = idmonhoc,
                DeThiId = iddethi
            };
            sd.MonHocDeThis.Add(abc);
            sd.SaveChanges();
        }
        /// <summary>
        /// dung de add cac cau hoi moi cua de moi
        /// </summary>
        /// <param name="noidungcauhoi"></param>
        public void AddCauHoiMoi(string tencauhoi,string noidungcauhoi)
        {
            CauHoi abc = new CauHoi()
            {
                TenCauHoi = tencauhoi,
                NoiDungCauHoi= noidungcauhoi
            };
            sd.Add(abc);
            sd.SaveChanges();
        }
        /// <summary>
        /// add dap an moi 
        /// </summary>
        /// <param name="noidungdapan"></param>
        public void DapAnMoi(string noidungdapan)
        {
            DapAn abc = new DapAn()
            {
                NoiDungDapAn = noidungdapan
            };
            sd.Add(abc);
            sd.SaveChanges();
        }
        /// <summary>
        /// add thong tin vao bang DetailDeThi
        /// </summary>
        /// <param name="tende"></param>
        /// <param name="idcauhoi"></param>
        /// <param name="iddapan"></param>
        public void AddLienKet (string tende, string cauhoi, string nddapan)
        {
            DeThi abc = sd.DeThis.Where(x => x.TenDeThi == tende).Single() ;
            int iddethi = abc.IddeThi;
           CauHoi xyz = sd.CauHois.Where(x => x.NoiDungCauHoi == cauhoi).Single();
            int idcauhoi = xyz.IdcauHoi;
           DapAn hhh = sd.DapAns.Where(x => x.NoiDungDapAn == nddapan).Single();
           int iddapan = hhh.IddapAn;
            
            DetailDeThi abc1 = new DetailDeThi()
            {
              
                DeThiId = iddethi,
            
                CauHoiId = idcauhoi,
           
                DapAnId = iddapan
            };
            
            sd.DetailDeThis.Add(abc1);
         
            abc = null;
            xyz = null;
            hhh = null;
            sd.SaveChanges();
        }
        /// <summary>
        /// sau khi tao xong dap an cau hoi cua 1 de moi thi add vafo bang answerdethi
        /// </summary>
        /// <param name="tende"></param>
        /// <param name="cauhoi"></param>
        /// <param name="nddapan"></param>
        public void AddAnswerDeThi(string tende,string cauhoi,string nddapan)
        {
            DeThi abc = sd.DeThis.Where(x => x.TenDeThi == tende).Single();
            int iddethi = abc.IddeThi;
            CauHoi xyz = sd.CauHois.Where(x => x.NoiDungCauHoi == cauhoi).Single();
            int idcauhoi = xyz.IdcauHoi;
            DapAn hhh = sd.DapAns.Where(x => x.NoiDungDapAn == nddapan).Single();
            int iddapan = hhh.IddapAn;
            AnswerDeThi abc1 = new AnswerDeThi()
            {
                DeThiId = iddethi,

                CauHoiId = idcauhoi,

                DapAnId = iddapan
            };
            sd.AnswerDeThis.Add(abc1);
            abc = null;
            xyz = null;
            hhh = null;
            sd.SaveChanges();
        }
        /// <summary>
        /// Add moi mon hoc doi voi giao vien
        /// </summary>
        /// <param name="monhoc"></param>
        public void AddNewMonHoc(string monhoc)
        {
            MonHoc abc = new MonHoc() { TenMonHoc = monhoc };
            sd.MonHocs.Add(abc);
            sd.SaveChanges();
        }
        public  void XoaDiemThi(int idacc, int idde)
        {
            Mark abc = sd.Marks.Where(x => x.HocSinhId == idacc & x.DeThiId == idde).Single();
            sd.Marks.Remove(abc);
            sd.SaveChanges();
        }
        /// <summary>
        /// Kiem tra xem co tai khoan khong de add
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Account KiemTra(string user)
        {
            Account abc = sd.Accounts.Where(x => x.Username == user).FirstOrDefault();
            return abc; 
        }
        public void AddTaiKhoan(string user, string pass)
        {
            Account abc = new Account()
            {
                Username = user,
                Password = pass,
                Role = "2" 
            };
            sd.Accounts.Add(abc);
            sd.SaveChanges();

        }
  
    }
}
