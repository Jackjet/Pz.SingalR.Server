using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Macrosage.Model;
using Macrosage.Utility;
using Macrosage.Utility.Cache;

namespace Macrosage.BLL.Core
{
    public class PushCache : IPushCache<GroupUser,UnReadMsg>
    {
        /// <summary>
        /// 设定组用户的cache
        /// </summary>
        /// <param name="groupUser"></param>
        /// <param name="groupId"></param>
        public GroupUser SetGroupUserCache(GroupUser groupUser, string groupId)
        {
            string groupAllUserKey = string.Format(MsCacheName.GetGroupAllUser, groupId);
           //设定cache
            return MsCacheHelper.InsertAndGetCache<GroupUser>(groupAllUserKey, groupUser);
        }
        /// <summary>
        /// 获取组用户cache
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public GroupUser GetGroupUserFromCache(string groupId)
        {
            string groupAllUserKey = string.Format(MsCacheName.GetGroupAllUser, groupId);
            var groupUser = MsCacheHelper.GetCache<GroupUser>(groupAllUserKey);
            if (groupUser == null)
            { 
                //从数据库获取，赋值给groupuser
                groupUser = UnReadDataBLL.Instance.GetEntMembers(groupId);
                if (groupUser != null && groupUser.clientUserIds.Count > 0)
                {
                    SetGroupUserCache(groupUser, groupId);
                }
            }
            return groupUser ?? new GroupUser() { GroupId = "0", clientUserIds = new List<string>() };
        }
        /// <summary>
        /// 私有方法，获取所有未读信息
        /// </summary>
        /// <returns></returns>
        private List<UnReadMsg> GetAllUnReadDataFromCache()
        {
            string unReadKey = MsCacheName.GetUserUnReadData;
            var unReadData = MsCacheHelper.GetCache<List<UnReadMsg>>(unReadKey);
            if (unReadData == null)
            {
               //从队列导入到数据库
                unReadData = ResetCache();
            }
            return unReadData ?? new List<UnReadMsg>();
        }
        /// <summary>
        /// 获取某个用户的未读消息集合
        /// </summary>
        /// <param name="clientUserId"></param>
        /// <returns></returns>
        public List<UnReadMsg> GetUnReadDataFromCache(string clientUserId)
        {
            return GetUnReadDataFromCacheByPredicate(x => x.clientUserId == clientUserId);
        }
        /// <summary>
        /// 设定用户未读消息
        /// </summary>
        /// <param name="unReadData"></param>
        public List<UnReadMsg> SetUnReadDataCache(List<UnReadMsg> unReadData)
        {
           return  MsCacheHelper.InsertAndGetCache<List<UnReadMsg>>(MsCacheName.GetUserUnReadData, unReadData);
        }
        /// <summary>
        /// 更新用户未读消息
        /// </summary>
        /// <param name="unReadUpdateData"></param>
        public void UpdateUnReadDataCache(List<UnReadMsg> unReadUpdateData)
        {
            string unReadKey = MsCacheName.GetUserUnReadData;
            var unReadData = MsCacheHelper.GetCache<List<UnReadMsg>>(unReadKey);
            if (unReadData == null)
            {
                ResetCache();
            }
            else
            {
                //遍历，如果有记录，那么加1，否则就添加一条
                foreach (var item in unReadUpdateData)
                {
                    var first = unReadData.FirstOrDefault(x => x.clientUserId == item.clientUserId && x.GroupId == item.GroupId && x.MessageType == item.MessageType);
                    if (first != null)
                    {
                        first.UnReadCount += 1;
                        first.AllCount += 1;
                    }
                    else
                    {
                        unReadData.Add(item);
                    }
                }
                //重置缓存数据
                SetUnReadDataCache(unReadData);
            }
        }

        private List<UnReadMsg> ResetCache()
        {
            //从队列导入到数据库
            new PushQueue().AddUnReadMsgToDbFromQueue(0);
            var  unReadData = UnReadDataBLL.Instance.GetUserUnReadData("0", "0", UtilityExt.EnumHelper.MessageType.MessageTypeNone);//从数据库获取
            if (unReadData != null || unReadData.Count > 0)
            {
                unReadData = SetUnReadDataCache(unReadData);
            }
            return unReadData;
        }

        /// <summary>
        /// 根据条件筛选相应的结果集
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<UnReadMsg> GetUnReadDataFromCacheByPredicate(Func<UnReadMsg, bool> predicate)
        {
            var unReadAllData = GetAllUnReadDataFromCache();
            return unReadAllData.Where(predicate).ToList();
        }

        /// <summary>
        /// 根据lambda表达式更新相关数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<UnReadMsg> UpdateUnReadDataFromCacheByPredicate(Func<UnReadMsg, bool> predicate,int count = 0)
        {
            string unReadKey = MsCacheName.GetUserUnReadData;
            var unReadData = MsCacheHelper.GetCache<List<UnReadMsg>>(unReadKey);
            if (unReadData == null)
            {
                ResetCache();
            }
            else
            {
                    //遍历
                    var list = unReadData.Where(predicate).ToList();
                    foreach (var item in list)
                    {
                        count = count == 0 ? item.UnReadCount : count;
                        unReadData.Add(new UnReadMsg
                        {
                            clientUserId = item.clientUserId,
                            UnReadCount = item.UnReadCount - count,
                            GroupId = item.GroupId,
                            AllCount = item.AllCount - count,//减去清零的那部分
                            MessageType = item.MessageType
                        });
                        unReadData.Remove(item);
                    }
                //重置缓存数据
                SetUnReadDataCache(unReadData);
            }
            return unReadData;
        }
    }
}