using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macrosage.BLL.Core
{
    /// <summary>
    /// 读取用户未读数据接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGetUnRead<T> where T : class,new()
    {
        /// <summary>
        /// 获取用户未读列表
        /// </summary>
        /// <param name="clientUserId"></param>
        /// <returns></returns>
        List<T> GetUserUnReadData(string clientUserId);
    }
}
