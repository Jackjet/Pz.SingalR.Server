using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pz.ChatServer.Core
{
    /// <summary>
    /// 操作缓存的方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPushCache<GU,UR> where GU:class,new() where UR :class,new()
    {
        /// <summary>
        /// 设置某个组织的所有人员
        /// </summary>
        /// <param name="dic"></param>
        void SetGroupUserCache(List<GU> groupUser,string groupId);
        /// <summary>
        /// 读取某个组织的所有成员
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        GU GetGroupUserFromCache(string groupId);
        /// <summary>
        /// 读取用户的未读记录
        /// </summary>
        /// <param name="clientUserId"></param>
        /// <returns></returns>
        List<UR> GetUnReadDataFromCache(string clientUserId);
        /// <summary>
        /// 设置用户的未读记录
        /// </summary>
        /// <param name="unReadData"></param>
        void SetUnReadDataCache(List<UR> unReadData);
        /// <summary>
        /// 更新部分用户未读记录
        /// </summary>
        /// <param name="unReadUpdateData"></param>
        void UpdateUnReadDataCache(List<UR> unReadUpdateData);
    }
}