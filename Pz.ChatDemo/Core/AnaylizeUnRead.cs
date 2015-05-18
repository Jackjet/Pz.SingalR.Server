using Pz.ChatServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pz.ChatServer.Core
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
    #endregion

       private UnReadMsg GetRecord(string clientUserId, string groupId, MessageType messageType)
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
        public bool AddUserToUserMsg(MessageType messageType, List<string> clientUserIds, string groupId)
        {
            List<string> outLineUserIds = new List<string>();

            //不在线的用户添加到outLineUserIds
            foreach (string clientUserId in clientUserIds)
            {
                if (!iAnaylizeOnlineUser.IsGroupOnline(clientUserId,groupId))
                {
                    outLineUserIds.Add(clientUserId);
                }
            }
            //如果都在线，不做任何操作
            if (outLineUserIds.Count == 0) { return true; }

            UnReadMsg oldMsg,newMsg;
            //加队列，加缓存
            //模拟加入队列
            foreach (var id in outLineUserIds)
            {
                oldMsg = GetRecord(id, groupId, messageType);
                //已经存在这条记录，就更新,不存在，加一条记录
                if (oldMsg == null)
                {
                    FakeQueueUnReadMsg.Add(new UnReadMsg
                    {
                        clientUserId = id,
                        GroupId = groupId,
                        MessageType = messageType,
                        UnReadCount = 1
                    });
                }
                else
                {
                    newMsg = new UnReadMsg
                    {
                        clientUserId = oldMsg.clientUserId,
                        GroupId = oldMsg.GroupId,
                        MessageType = oldMsg.MessageType,
                        UnReadCount = oldMsg.UnReadCount + 1
                    };
                    UpdateUserUnReadCount(oldMsg, newMsg);
                }
            }
            return true;
        }
        /// <summary>
        /// 消息推送到组织添加未读
        /// 根据组织Id操作内部业务逻辑，获取未在线名单
        /// </summary>
        /// <param name="messageType">消息类型</param>
        /// <param name="groupId">组织Id</param>
        /// <returns>返回操作成功True 操作失败False</returns>
        public bool AddUserToGroupMsg(MessageType messageType, string groupId)
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
        public bool UpdateUserUnReadCount(MessageType messageType, string clientUserId, int count = 0)
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
            FakeQueueUnReadMsg.RemoveAll(x => x.clientUserId == clientUserId);
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
            FakeQueueUnReadMsg.RemoveAll(x => x.clientUserId == clientUserId && x.GroupId == groupId);
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
            FakeQueueUnReadMsg.Remove(oldMsg);
            FakeQueueUnReadMsg.Add(newMsg);
            return true;
        }
    }
}