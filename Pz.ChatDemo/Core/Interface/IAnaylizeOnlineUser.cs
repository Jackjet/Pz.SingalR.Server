using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pz.ChatServer.Core
{
   public interface IAnaylizeOnlineUser<T> where T:class,new()
    {
       void setOnlineUser(IList<T> onlineUser);
       /// <summary>
       /// 获取当前组用户的在线个数
       /// </summary>
       /// <param name="groupId">组唯一标识</param>
       /// <returns></returns>
       int GetOnlineUserCount(string groupId);
       /// <summary>
       /// 获取当前在线用户的userId集合
       /// </summary>
       /// <param name="groupId">组唯一标识</param>
       /// <returns></returns>
       IList<string> GetOnlineUserIds(string groupId);
       /// <summary>
       /// 获取组内在线的人员列表
       /// </summary>
       /// <param name="groupId">组唯一标识</param>
       /// <returns></returns>
       IList<T> GetGourpOnlineUsers(string groupId);
       /// <summary>
       /// 获取所有在线的人员个数
       /// </summary>
       /// <returns></returns>
       int GetAllOnlineUserCount();
       /// <summary>
       /// 获取所有在线的人员列表ID
       /// </summary>
       /// <returns></returns>
       IList<string> GetOnlineUserIds();
       /// <summary>
       /// 获取所有在线人员列表
       /// </summary>
       /// <returns></returns>
       IList<T> GetAllOnlineUsers();
       /// <summary>
       /// 判断某个用户是否在线
       /// </summary>
       /// <param name="clientUserId"></param>
       /// <returns></returns>
       bool IsOnline(string clientUserId);
       /// <summary>
       /// 判断某个用户在某个组是否在线
       /// </summary>
       /// <param name="clientUsrId"></param>
       /// <param name="groupId"></param>
       /// <returns></returns>
       bool IsGroupOnline(string clientUserId, string groupId);
    }
}
