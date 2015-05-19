using Macrosage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Macrosage.UtilityExt;
using Macrosage.Utility;

namespace Macrosage.BLL.Core
{
    public class AnaylizeUnRead : IAnaylizeUnRead<UnReadMsg>
    {
       #region 私有变量
       private IAnaylizeOnlineUser<OnlineUser> _iAnaylizeOnlineUser;
       private IPushCache<GroupUser, UnReadMsg> _iPushCache;
       private IPushCache<GroupUser, UnReadMsg> iPushCache
       {
           get
           {
               if (_iPushCache == null)
               {
                   _iPushCache = new PushCache();
               }
               return _iPushCache;
           }
       }
       private IAnaylizeOnlineUser<OnlineUser> iAnaylizeOnlineUser
       {
           get
           {
               if (_iAnaylizeOnlineUser == null)
               {
                   _iAnaylizeOnlineUser = new Anaylize(ChatHub.OnLineUser);
               }
               return _iAnaylizeOnlineUser;
           }
           
       }
       private PushQueue _pushQueue;
       private PushQueue pushQueue
       {
           get
           {
               if (_pushQueue == null)
               {
                   _pushQueue = new PushQueue();
               }
               return _pushQueue;
           }
       }
    #endregion

       private UnReadMsg GetRecord(string clientUserId, string groupId,EnumHelper.MessageType messageType)
       {
           var first = FakeQueueUnReadMsg.FirstOrDefault(x => x.clientUserId == clientUserId && x.MessageType == messageType && x.GroupId == groupId);
           return first;
       }
        //模拟队列
       public static List<UnReadMsg> FakeQueueUnReadMsg = new List<UnReadMsg>();
       public AnaylizeUnRead()
        {
            //是否单例，优化
           // iPushCache = new PushCache();
        }
        /// <summary>
        /// 添加未读消息信息
        /// 人员筛选业务包括在内，将已在线的去掉
        /// </summary>
        /// <param name="messageType">消息类型</param>
        /// <param name="clientUserIds">接收到的人员Id</param>
        ///<param name="clientUserIds">组织Id</param>
        /// <returns>返回操作成功True 操作失败False</returns>
       public bool AddUserToUserMsg(EnumHelper.MessageType messageType, List<string> clientUserIds, string groupId)
       {
           List<string> outLineUserIds = new List<string>();

           //不在线的用户添加到outLineUserIds
           foreach (string clientUserId in clientUserIds)
           {
               if (!iAnaylizeOnlineUser.IsGroupOnline(clientUserId, groupId))
               {
                   outLineUserIds.Add(clientUserId);
               }
           }
           //如果都在线，不做任何操作
           if (outLineUserIds.Count == 0) { return true; }
           //加队列，加缓存

           UnReadMsg dbMsg;
           var list = new List<UnReadMsg>();
           foreach (var id in outLineUserIds)
           {
               dbMsg = new UnReadMsg
                   {
                       clientUserId = id,
                       GroupId = groupId,
                       MessageType = messageType,
                       UnReadCount = 1,
                       AllCount = 1
                   };
               list.Add(dbMsg);
           }
           //将未读消息添加到缓存
           iPushCache.UpdateUnReadDataCache(list);
           //将未读消息添加到队列
           return pushQueue.AddUnReadMsgToQueue(list);
       }
        /// <summary>
        /// 消息推送到组织添加未读
        /// 根据组织Id操作内部业务逻辑，获取未在线名单
        /// </summary>
        /// <param name="messageType">消息类型</param>
        /// <param name="groupId">组织Id</param>
        /// <returns>返回操作成功True 操作失败False</returns>
       public bool AddUserToGroupMsg(EnumHelper.MessageType messageType, string groupId)
        {
            GroupUser groupAllUser = iPushCache.GetGroupUserFromCache(groupId);
            return AddUserToUserMsg(messageType, groupAllUser.clientUserIds, groupId);
        }

       /// <summary>
       /// 更新某个用户的未读消息
       /// </summary>
       /// <param name="messageType">消息类型</param>
       /// <param name="clientUserId">用户ID</param>
       /// <param name="count">count+=count</param>
        /// <returns>返回操作成功True 操作失败False</returns>
       public bool UpdateUserUnReadCount(EnumHelper.MessageType messageType, string clientUserId, int count = 0)
        {
          // FakeQueueUnReadMsg.FirstOrDefault()
            return true;
        }
        /// <summary>
        /// 清空某个用户的未读消息
        /// </summary>
        /// <param name="clientUserId">用户ID</param>
        /// <returns>返回操作成功True 操作失败False</returns>
        public bool ClearUserAllUnReadInfo(string clientUserId)
        {
            return true;
        }
        /// <summary>
        /// 清空某个用户在某个组织的未读消息
        /// </summary>
        /// <param name="clientUserId">用户ID</param>
        /// <param name="groupId">组织ID</param>
        /// <returns>返回操作成功True 操作失败False</returns>
        public bool ClearUserGroupUnReadInfo(string clientUserId, string groupId)
        {
            return true;
        }

        /// <summary>
        /// 用新的实体代替旧的实体
        /// </summary>
        /// <param name="oldMsg"></param>
        /// <param name="newMsg"></param>
        /// <returns></returns>
        public bool UpdateUserUnReadCount(UnReadMsg oldMsg, UnReadMsg newMsg)
        {
            //int count = MsQueue.Instance(UnReadQueueId).QueueMessageCount;
            //List<UnReadMsg> list = new List<UnReadMsg>();
            //for (int i = 0; i < count; i++)
            //{
            //    var model = MsQueue.Instance(UnReadQueueId).ReceiveByBit<UnReadMsg>();
            //    list.Add(model);
            //}
            //list.Remove(oldMsg);
            //list.Add(newMsg);
            //foreach (var item in list)
            //{
            //    MsQueue.Instance(UnReadQueueId).SendByBit<UnReadMsg>(item);
            //}
            return true;
        }

        /// <summary>
        /// 用户进入组织内调用，清除掉用户普通信息未读数据
        /// </summary>
        /// <param name="clientUserId">用户ID</param>
        /// <param name="groupId">组织ID</param>
        /// <returns>返回是否成功</returns>
        public bool ClearUserGroupUnReadInfoOfTypeNormal(string clientUserId, string groupId)
        {
            //先更新数据库，在更新缓存
            pushQueue.AddUnReadMsgToDbFromQueue(0);
            UnReadDataBLL.Instance.UpdateUserUnReadData(clientUserId, groupId, EnumHelper.MessageType.MessageTypeNormal);
           iPushCache.UpdateUnReadDataFromCacheByPredicate(x => x.clientUserId == clientUserId && x.GroupId == groupId && x.MessageType == EnumHelper.MessageType.MessageTypeNormal);
           return true;
        }
        /// <summary>
        /// 用户查看某个文章，将文章未读信息减去1
        /// </summary>
        /// <param name="clientUserId">用户ID</param>
        /// <param name="groupId">用户组织号</param>
        /// <returns>返回是否成功</returns>
        public bool ClearUserGroupUnReadInfoOfTypeArticle(string clientUserId, string groupId,int count = 1)
        {
            //先更新数据库，在更新缓存
            pushQueue.AddUnReadMsgToDbFromQueue(0);
            UnReadDataBLL.Instance.UpdateUserUnReadData(clientUserId, groupId, EnumHelper.MessageType.MessageTypeArticle,count);
            iPushCache.UpdateUnReadDataFromCacheByPredicate(x => x.clientUserId == clientUserId && x.GroupId == groupId && x.MessageType == EnumHelper.MessageType.MessageTypeArticle,count);
            return true;
        }
    }
}