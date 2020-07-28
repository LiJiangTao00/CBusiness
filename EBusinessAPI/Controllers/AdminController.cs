using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.Models;
using Model.ViewModel;
using Newtonsoft.Json;

namespace EBusinessAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private PracticalContext context;
        public AdminController(PracticalContext context)
        {
            this.context = context;
        }
        JWTHelper jwt = new JWTHelper();
        /// <summary>
        /// 用户等级显示积分显示
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ShowMemberGrade(int page = 1, int pageSize = 5)
        {
            List<Customer> customers = context.Customer.ToList();
            List<OrderInfo> orders = context.OrderInfo.ToList();
            var q = from o in orders
                    group o by o.UserId into g
                    select new
                    {
                        g.Key,
                        TotalPrice = g.Sum(o => o.Totalprice)
                    };
            List<MemberGrade> members = (from c in customers
                                         join o in q on c.UserId equals o.Key
                                         into re
                                         from r in re.DefaultIfEmpty()
                                         select new MemberGrade
                                         {
                                             UserId = c.UserId,
                                             Id = c.UserId,
                                             UserName = c.UserName,
                                             Totalprice = (r == null ? 0 : r.TotalPrice),
                                             Userlevel = c.Userlevel,
                                             Useraddress = c.Useraddress
                                         }).ToList();

            PageModel<MemberGrade> model = new PageModel<MemberGrade>
            {
                data = members.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                count = members.Count(),
                code = 0,
                msg = ""
            };
            return Ok(model);
        }
        /// <summary>
        /// 未用修改等级
        /// </summary>
        /// <param name="intSize"></param>
        /// <returns></returns>
        [HttpPost]
        public int UptMemberGrade(string intSize)
        {
            int flag = 0;
            string[] vs = intSize.Split(',');
            foreach (var item in vs)
            {
                switch (Convert.ToInt32(item) / 1000)
                {
                    case 1:
                        ForMemberGrade(1);
                        break;
                    case 2:
                        ForMemberGrade(2);
                        break;
                    case 3:
                        ForMemberGrade(3);
                        break;
                    case 4:
                        ForMemberGrade(4);
                        break;
                    case 5:
                        ForMemberGrade(5);
                        break;
                    case 6:
                        ForMemberGrade(6);
                        break;
                    case 7:
                        ForMemberGrade(7);
                        break;
                    case 8:
                        ForMemberGrade(8);
                        break;
                    case 9:
                        ForMemberGrade(9);
                        break;
                    case 10:
                        ForMemberGrade(10);
                        break;
                    default:
                        ForMemberGrade(11);
                        break;
                }
                flag = context.SaveChanges();
            }
            return flag;
        }
        /// <summary>
        /// 未用修改等级调用方法
        /// </summary>
        /// <param name="i"></param>
        public void ForMemberGrade(int i)
        {
            List<Customer> customers = context.Customer.ToList();
            List<OrderInfo> orders = context.OrderInfo.ToList();
            List<MemberGrade> members = (from c in customers
                                         join o in orders on c.UserId equals o.UserId
                                         select new MemberGrade
                                         {
                                             UserId = c.UserId,
                                             Id = c.UserId,
                                             UserName = c.UserName,
                                             Totalprice = o.Totalprice,
                                             Userlevel = c.Userlevel,
                                             Useraddress = c.Useraddress
                                         }).ToList();
            MemberGrade member = members.Find(x => float.Parse(x.Totalprice.ToString()) / 1000 == i);
            Customer admin = context.Customer.ToList().Find(a => a.UserId == member.UserId);
            switch (i)
            {
                case 1:
                    admin.Userlevel = "一般挽留客户";
                    break;
                case 2:
                    admin.Userlevel = "一般保持客户";
                    break;
                case 3:
                    admin.Userlevel = "一般发展客户";
                    break;
                case 4:
                    admin.Userlevel = "一般价值客户";
                    break;
                case 5:
                    admin.Userlevel = "重要挽留客户";
                    break;
                case 6:
                    admin.Userlevel = "重要保持客户";
                    break;
                case 7:
                    admin.Userlevel = "重要保持客户";
                    break;
                case 8:
                    admin.Userlevel = "重要保持客户";
                    break;
                case 9:
                    admin.Userlevel = "重要发展客户";
                    break;
                case 10:
                    admin.Userlevel = "重要发展客户";
                    break;
                default:
                    admin.Userlevel = "重要价值客户";
                    break;
            }
            context.Entry(member).State = EntityState.Modified;
        }
        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost]
        public int RegistCustonmer(Customer customer)
        {
            customer.Userlevel = "一般挽留客户";
            customer.UserPwd = Md5Helper.ToMd5(customer.UserPwd);
            context.Customer.Add(customer);
            return context.SaveChanges();
        }
        /// <summary>
        /// 登录用户
        /// </summary>
        /// <param name="Cname">账号</param>
        /// <param name="Cpwd">密码</param>
        /// <returns></returns>
        [HttpGet]
        public int LoginCustonmer(string Cname, string Cpwd)
        {
            Cpwd = Md5Helper.ToMd5(Cpwd);
            Customer customer = context.Customer.ToList().Find(x => x.UserName.Equals(Cname) && x.UserPwd.Equals(Cpwd));
            if (customer != null)
            {
                return 1;
            }
            return 0;
        }
        [HttpGet]
        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="Aname">账号</param>
        /// <param name="Apwd">密码</param>
        /// <returns></returns>
        public int LoginAdmin(string Aname, string Apwd)
        {
            Apwd = Md5Helper.ToMd5(Apwd);
            AdminInfo admin = context.AdminInfo.ToList().Find(x => x.AdminName.Equals(Aname) && x.AdminPwd.Equals(Apwd));
            if (admin != null)
            {
                return 1;
            }
            return 0;
        }
        /// <summary>
        /// 保存到redis
        /// </summary>
        /// <param name="ProductTypeName">商品模块的类型</param>
        /// <returns></returns>
        public bool SaveRedis(string ProductTypeName)
        {
            RedisHelper redisHelper = new RedisHelper("127.0.0.1:6379");
            bool testValue = false;
            if (ProductTypeName == "新商品")
            {
                //前十条最新商品
                List<ProductInfo> newProduct = (from p in context.ProductInfo.ToList() orderby p.ProductStoretime descending select p).ToList();
                string newProductString = JsonConvert.SerializeObject(newProduct.Take(10));
                //存储到redis
                testValue = redisHelper.SetValue("newProduct", newProductString);
            }
            else if (ProductTypeName == "促销商品")
            {
                List<ProductInfo> AllProduct = (from p in context.ProductInfo.ToList() orderby p.ProductStoretime descending select p).ToList();

                string lowProductString = JsonConvert.SerializeObject(AllProduct);
                //存储到redis
                testValue = redisHelper.SetValue("lowProduct", lowProductString);
            }
            else if (ProductTypeName == "热销商品")
            {
                List<ProductInfo> newProduct = (from p in context.ProductInfo.ToList() orderby p.ProductDealamount descending select p).ToList();
                string hotProductString = JsonConvert.SerializeObject(newProduct.Take(20));
                //存储到redis
                testValue = redisHelper.SetValue("hotProduct", hotProductString);
            }
            else if (ProductTypeName == "本月top10")
            {
                //浏览次数最多
                List<ProductInfo> newProduct = (from p in context.ProductInfo.ToList() orderby p.ProductLookamount descending select p).ToList();
                string lookProductString = JsonConvert.SerializeObject(newProduct.Take(10));
                //存储到redis
                testValue = redisHelper.SetValue("lookProduct", lookProductString);
            }
            else
            {

            }
            return testValue;
        }
        /// <summary>
        /// 获取redis中保存的各种商品
        /// </summary>
        /// <param name="ProductTypeName">商品模块类型</param>
        /// <returns></returns>
        public List<ProductInfo> GetRedis(string ProductTypeName)
        {
            RedisHelper redisHelper = new RedisHelper("127.0.0.1:6379");
            ///获取redis
            string saveValue = "";
            if (ProductTypeName == "新商品")
            {
                ///获取redis
                saveValue = redisHelper.GetValue("newProduct");
            }
            else if (ProductTypeName == "促销商品")
            {
                ///获取redis
                saveValue = redisHelper.GetValue("lowProduct");
            }
            else if (ProductTypeName == "热销商品")
            {
                ///获取redis
                saveValue = redisHelper.GetValue("hotProduct");
            }
            else if (ProductTypeName == "本月top10")
            {
                //浏览次数最多
                ///获取redis
                saveValue = redisHelper.GetValue("lookProduct");
            }
            else
            {

            }
            if (saveValue==null)
            {
                return null;
            }
            List<ProductInfo> products = JsonConvert.DeserializeObject<List<ProductInfo>>(saveValue);
            return products;
        }
        /// <summary>
        /// 修改或添加等级
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        [HttpPost]
        public bool SetGrade(GradeModel m)
        {
            RedisHelper redisHelper = new RedisHelper("127.0.0.1:6379");
            string saveGrade = redisHelper.GetValue("saveGrade");
            bool flag = false;
            List<GradeModel> grades = null;
            if (saveGrade != "[]")
            {
                grades = JsonConvert.DeserializeObject<List<GradeModel>>(saveGrade);
                GradeModel grade = grades.Find(a => a.GName == m.GName);
                if (grade != null)
                {
                    grades.Remove(grade);
                }
            }
            else
            {
                grades = new List<GradeModel>();
            }
            grades.Add(m);
            grades = (from g in grades orderby g.GSize ascending select g).ToList();
            flag = redisHelper.SetValue("saveGrade", JsonConvert.SerializeObject(grades));
            return flag;
        }
        /// <summary>
        /// 显示等级
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetGrade()
        {
            RedisHelper redisHelper = new RedisHelper("127.0.0.1:6379");
            string saveGrade = redisHelper.GetValue("saveGrade");
            List<GradeModel> grades = null;
            if (!string.IsNullOrEmpty(saveGrade))
            {
                grades = JsonConvert.DeserializeObject<List<GradeModel>>(saveGrade);
                grades = (from g in grades orderby g.GSize ascending select g).ToList();
            }
            PageModel<GradeModel> model = new PageModel<GradeModel>
            {
                data = grades,
                count = grades.Count
            };
            return Ok(model);
        }
        /// <summary>
        /// 删除等级
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        [HttpGet]
        public bool ReSetGrade(string GSize)
        {
            RedisHelper redisHelper = new RedisHelper("127.0.0.1:6379");
            string saveGrade = redisHelper.GetValue("saveGrade");
            bool flag = false;
            List<GradeModel> grades = null;
            if (saveGrade != "[]")
            {
                grades = JsonConvert.DeserializeObject<List<GradeModel>>(saveGrade);
            }
            else
            {
                grades = new List<GradeModel>();
            }
            GradeModel grade = grades.Find(a => a.GSize ==int.Parse(GSize));
            grades.Remove(grade);
            flag = redisHelper.SetValue("saveGrade", JsonConvert.SerializeObject(grades));
            return flag;
        }
        /// <summary>
        /// 用户游客登录
        /// </summary>
        /// <param name="Cname"></param>
        /// <param name="Cpwd"></param>
        /// <returns></returns>
        public string LoginCustomer(string Cname,string Cpwd)
        {
            string token = "";
            Customer customer = context.Customer.ToList().Find(a=>a.UserName==Cname&&a.UserPwd==Cpwd);
            if (customer!=null)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("UserId", customer.UserId);
                dic.Add("UserName", customer.UserName);
                dic.Add("Userlevel", customer.Userlevel);
                token = jwt.GetToken(dic, 30000);
            }
            else
            {
                token = "";
            }
            return token;
        }
        /// <summary>
        /// 分解token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Customer ResolveToken(string token)
        {
            string json = jwt.GetPayload(token);
            Customer customer = JsonConvert.DeserializeObject<Customer>(json);
            return customer;
        }
        [HttpGet]
        public List<ProductTypeInfo> ShowType()
        {
            List<ProductTypeInfo> productTypes = context.ProductTypeInfo.ToList();
            return productTypes;
        }
    }
}
