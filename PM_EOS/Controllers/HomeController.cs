using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PM_EOS.Models;
using PM_EOS.Mutiple;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PM_EOS.Controllers
{
    public class HomeController : Controller
    {
        Caulenh _fun = new Caulenh();
        // giao dien man hinh dang nhap
        public IActionResult Giaodiendangnhap()
        {
            ViewBag.err = HttpContext.Session.GetString("err");
            return View();
        }
        // Back End xu ly danh nhap
        [HttpPost]
        public IActionResult DangNhap()
        {
            string username = HttpContext.Request.Form["user"];
            string pass = HttpContext.Request.Form["pass"];
            Account acc = _fun.GetAcc(username, pass);
            if(acc == null)
            {
                HttpContext.Session.SetString("err", "tai khoan hoac mat khau chua dung ");
                return Redirect("/Home/Giaodiendangnhap");
            }
            else
            {
                HttpContext.Session.SetString("iduser", acc.Idacc.ToString());
                HttpContext.Session.SetString("username", acc.Username);
                HttpContext.Session.SetString("role", acc.Role);
                return Redirect("/Home/TrangChu");
            }
        }
        // trang chu dang nhap
        public IActionResult TrangChu()
        {
            string role= HttpContext.Session.GetString("role");
            if (role == "1") {
                ViewBag.role = "Giao vien";
                    }
            else
            {
                ViewBag.role = "Hoc sinh";
            }
            ViewBag.user = HttpContext.Session.GetString("username");
            List<MonHoc> list = _fun.GetAllMonHoc();
            ViewData["list"] = list;
            ViewBag.thongbao = HttpContext.Session.GetString("thongbao");
            return View();
        }
        // dang xuat tai khoan xong thi se quay ve trang giaodiendangnhap
        public IActionResult DangXuat()
        {
            HttpContext.Session.Clear();
            return Redirect("/Home/Giaodiendangnhap");
        }
     
        public IActionResult DeThi(int id )
        {
            int idde = Convert.ToInt32(id);
            List<LKMonhocDeThi> abc = _fun.GetAllDeThi(idde);

            return View(abc);
        }
        // hien thi len nhung de thi nao dang bi khoa va dang mo
        public IActionResult GiaoDienMoDongDeThi()
        {
            List<DeThi> lock1 = _fun.DeThiLock();
            List<DeThi> unlock = _fun.DeThiUnLock();
            ViewData["lock"] = lock1;
            ViewData["unlock"] = unlock;
            return View();
        }
        // su kien nay dung de mo khoa de thi dang o trang thai lock
        public IActionResult MoKhoaDeThi(int id)
        {
            _fun.ToUnLock(id);
            return Redirect("/Home/GiaoDienMoDongDeThi");
        }
        // su kien nay dung de khoa de thi dang o trong trang thai unlock
        public IActionResult KhoaDeThi(int id)
        {
            _fun.ToLock(id);
            return Redirect("/Home/GiaoDienMoDongDeThi");
        }
        // giao dien trang vao thi
        public IActionResult  GiaoDienTrangThi(int idde,int Monthi)
        {
            // lay ra cau hoi 1 de
            List<CauHoi> abc = _fun.GetCauHoi1De(idde);
            List<CauHoi_DetailDeThi_DapAn> list = _fun.GetDeThi(idde);
            ViewData["list"] = list;
            ViewData["abc"] = abc;
            ViewBag.idde = idde;
            HttpContext.Session.SetString("Monthi", Monthi.ToString());
            string monthi = HttpContext.Session.GetString("Monthi");
            return View();
        }
        // xu ly sau khi tinh diem
        [HttpPost]
        public IActionResult NopBai(string idde)
        {
            int id = Convert.ToInt32(idde);
            List<AnswerDeThi> list = _fun.DapAn1De(id);
            int count = list.Count();
            List<string> xyz = new List<string>();
            for (int i =1; i <= count; i++)
            {
                string request = HttpContext.Request.Form["radio" + i];
                if(request == null)
                {
                    request = "0";
                }
                xyz.Add(request);
            }
            int sum = 0;
            for(int a = 0; a < count; a++)
            {
                if (list[a].DapAnId.ToString().Equals(xyz[a]))
                {
                    sum++;
                }
            }
            double tinhdiem = sum * 10 / (double)count;
            double xyz1 = Math.Round(tinhdiem, 2);
            ViewBag.diem = xyz1;
            ViewBag.idde = idde;
            return View();
        }
        /// <summary>
        /// hello
        /// </summary>
        /// <param name="diem"></param>
        /// <param name="idde"></param>
        /// <returns></returns>
        // ham nay dung de add diem sau khi hoc sinh thi xong
        [HttpPost]
        public IActionResult SaveDiemSauKhiThiXong(string diem, int idde)
        {
            int idacc = Convert.ToInt32(HttpContext.Session.GetString("iduser"));
            int monhoc = Convert.ToInt32(HttpContext.Session.GetString("Monthi"));
            double diemthi = Math.Round(double.Parse(diem), 0);
            int diem1 = Convert.ToInt32(diemthi);
            int dethi = idde;
            _fun.SaveDiemThi(idacc, monhoc, dethi, diem1);
         
            return Redirect("/Home/TrangChu");
        }
        // lay ra diem cua 1 hoc sinh - dung cho hoc sinh
        public IActionResult HocSinhXemDiem() {
            string idhs = HttpContext.Session.GetString("iduser");
        
            ViewBag.name = HttpContext.Session.GetString("username");
            List<MuitipleDiemThi> abc = _fun.Diem1HocSinh(idhs);
            return View(abc);
        }
        /// <summary>
        /// Lay ra diem của tất cả học sinh
        /// </summary>
        /// <returns></returns>
        public IActionResult GetAllDiem()
        {
            string idhs = HttpContext.Session.GetString("iduser");

            ViewBag.name = HttpContext.Session.GetString("username");
            List<MuitipleDiemThi> abc = _fun.DiemAllHocSinh();
            return View(abc);
        }
        /// <summary>
        /// Tao de thi do giao vien tao de
        /// </summary>
        /// <returns></returns>
        public IActionResult TaoDeThi()
        {
            List<MonHoc> abc = _fun.GetAllMonHoc();
            ViewData["monhoc"] = abc;
            return View();
        }
        /// <summary>
        /// tao cau hoi  theo mon hoc va de thi 
        /// </summary>
        /// <returns></returns>
        public IActionResult TaoCauHoi()
        {
            string monhoc = HttpContext.Request.Form["monhoc"];
            string tendethi = HttpContext.Request.Form["tendethi"];
            string soluongcau = HttpContext.Request.Form["soluong"];
            // add de thi moi
         //   _fun.AddDeThi(tendethi);
            // add de thi vua tao ra voi
            int idmonhoc = Convert.ToInt32(monhoc);
             DeThi abc = _fun.Lay1DeThi(tendethi);
         //   _fun.AddLienKetMonDe(idmonhoc, abc.IddeThi);


            ViewBag.tende = tendethi;
            ViewBag.soluongcau = soluongcau;
            return View();
        }
        /// <summary>
        ///  ham nay dung de Luu Lai cac cau hoi cua de thi moi
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SaveDeThiMoi(int soluong,string tende)
        {
            int soluong1 = soluong * 3;
            // dung add tat  ca cac dap an vao 1 
            List<string> listiddapan = new List<string>();
            for(int count =1; count <= soluong1; count++)
            {
                string dapan = "";
                if (count %3 ==1)
                {
                    dapan = "A :" + HttpContext.Request.Form[count.ToString()];
                }
                if (count % 3== 2)
                {
                    dapan = "B :" + HttpContext.Request.Form[count.ToString()];
                }
                if (count % 3== 0)
                {
                    dapan = "C :" + HttpContext.Request.Form[count.ToString()];
                }

                listiddapan.Add(dapan);
                _fun.DapAnMoi(dapan);
            }
            // add cac cau hoi moi
            List<string> cauhoi = new List<string>();
            for(int count =1; count <= soluong; count++)
            {
                string cau = "Causo" + count;
                cauhoi.Add(HttpContext.Request.Form[cau]);
                _fun.AddCauHoiMoi(cau,HttpContext.Request.Form[cau]);
            }

            // add cac dap an dung vao 1 cai list roi de con add vao bang AnswerDeThi
            List<string> huhu = new List<string>();
            for(int i = 1; i<= soluong;i++)
            {
                string ketqua = HttpContext.Request.Form["Dapan"+i];
                string[] catchuoi = ketqua.Split(' ');
                huhu.Add(catchuoi[1]);
            }
            for (int count = 1;count <= huhu.Count;count++) 
            {
                _fun.AddAnswerDeThi(tende, cauhoi[count - 1], listiddapan[Convert.ToInt32(huhu[count - 1])-1]);
            
            }

            // add vao dap an , ten de, cau hoi vao 1 DetailDeThi
            int soluot = 0;
            for (int count = 0; count < listiddapan.Count; count++)
            {
              
                if(  count >0 && count % 3 == 0)
                {
                    soluot++;
                }
                _fun.AddLienKet(tende, cauhoi[soluot], listiddapan[count]);
            }
            HttpContext.Session.SetString("thongbao", "ban vua add thanh cong de thi moi");
            return Redirect("/Home/TrangChu");
        }  
        /// <summary>
        /// Giao dien tao mon hoc moi
        /// </summary>
        /// <returns></returns>
        public IActionResult GiaoDienTaoMonHoc()
        {
            ViewBag.thongbao = HttpContext.Session.GetString("hihi");
            return View();
            
        }
        /// <summary>
        /// Back End xu ly add moi mon hoc
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult XuLyAddMonHoc()
        {
            string monhoc = HttpContext.Request.Form["monhoc"];
            _fun.AddNewMonHoc(monhoc);
            HttpContext.Session.SetString("hihi", "ban da add thanh cong 1 mon hoc moi");
            return Redirect("/Home/GiaoDienTaoMonHoc");
        }
        public IActionResult Giaodiensuadethi()
        {
            return View();
        }
        /// <summary>
        /// Dung de giao vien xoa diem thi hoc sinh
        /// </summary>
        /// <param name="idacc"></param>
        /// <param name="idde"></param>
        /// <returns></returns>
        public IActionResult XoaDiemThi(int idacc, int idde)
        {
           
             _fun.XoaDiemThi(idacc, idde);
            return Redirect("/Home/GetAllDiem");
        }
        /// <summary>
        /// Giao dien dang ki tai khoan moi
        /// </summary>
        /// <returns></returns>
        public  IActionResult GiaoDienDangKiTaiKhoan()
        {
            ViewBag.tbtk = HttpContext.Session.GetString("loitk");
            HttpContext.Session.Clear();
            return View();
        }
        [HttpPost]
       /// <summary>
       /// back end xu ly viec dang ki tai khoan moi
       /// </summary>
       /// <returns></returns>
       
        public IActionResult XuLyDangKiTaiKhoan()
        {
            string user = HttpContext.Request.Form["user"];
            string pass = HttpContext.Request.Form["pass"];
            string pass1 = HttpContext.Request.Form["pass1"];
            if(pass != pass1)
            {
                HttpContext.Session.SetString("loitk", "Mat khau khong giong nhau ");
            }
            else
            {
                Account aaa = _fun.KiemTra(user);
                if(aaa == null)
                {
                    HttpContext.Session.SetString("loitk", "Add thanh cong tai khoan moi ");
                    _fun.AddTaiKhoan(user,pass);
                }
                else
                {
                    HttpContext.Session.SetString("loitk", "Tai khoan nay da duoc dang ki ");
                }
            }
   
            return Redirect("/Home/GiaoDienDangKiTaiKhoan");
        }

    }
}
