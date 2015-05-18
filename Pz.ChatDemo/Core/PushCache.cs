using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pz.ChatServer.Model;

namespace Pz.ChatServer.Core
{
    public class PushCache : IPushCache<GroupUser,UnReadMsg>
    {
        /// <summary>
        /// 设定组用户的cache
        /// </summary>
        /// <param name="groupUser"></param>
        /// <param name="groupId"></param>
        public void SetGroupUserCache(List<GroupUser> groupUser, string groupId)
        {
           //设定cache
        }
        /// <summary>
        /// 获取组用户cache
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public GroupUser GetGroupUserFromCache(string groupId)
        {
            return new GroupUser
            {
                GroupId = "90000710",
                clientUserIds = new List<string> {"10001","10002","10003","10004","10005","10006"}
            };
        }
        /// <summary>
        /// 获取某个用户的未读消息集合
        /// </summary>
        /// <param name="clientUserId"></param>
        /// <returns></returns>
        public List<UnReadMsg> GetUnReadDataFromCache(string clientUserId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 设定用户未读消息
        /// </summary>
        /// <param name="unReadData"></param>
        public void SetUnReadDataCache(List<UnReadMsg> unReadData)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 更新用户未读消息
        /// </summary>
        /// <param name="unReadUpdateData"></param>
        public void UpdateUnReadDataCache(List<UnReadMsg> unReadUpdateData)
        {
            throw new NotImplementedException();
        }
    }
}