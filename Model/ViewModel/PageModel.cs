using System;
using System.Collections.Generic;
using System.Text;

namespace Model.ViewModel
{
    public class PageModel<T>
    {
        /// <summary>
        /// 数量
        /// </summary>
        public int count { get; set; } = 0;
        /// <summary>
        /// 响应码
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public List<T> data { get; set; }
    }
}
