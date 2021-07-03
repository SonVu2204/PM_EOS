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
            return View();
        }
    }
}
