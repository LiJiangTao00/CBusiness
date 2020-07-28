using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EBAdmin.Controllers
{
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        public IActionResult Show()
        {
            return View();
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        public IActionResult Showtest()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public IActionResult test()
        {
            return View();
        }
    }
}
