using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Pz.ChatServer.Core
{
    public class ChatHub : Hub  
    {
        public string CurretnGroupName { get; set; }
        /// <summary>
        /// 当前连接用户的ConnectionId
        /// </summary>
        public string CurrentGroupUserConnectionId
        {
            get
            {
                return Context.ConnectionId;
            }
        }
        public static List<OnlineUser> OnLineUser { get; set; }
        /// <summary>
        /// 当前所有在线用户个数
        /// </summary>
        public long GroupOnLineUserCount
        {
            get
            {
                if (ChatHub.OnLineUser == null)
                {
                    return 0;
                }
                return ChatHub.OnLineUser.Where(x => x.groupName == CurretnGroupName).ToList().Count;
            }
        }

        #region 服务端自动调用 显示连接状态
        private BaseMessage getSysMsg(string message, string title = "系统消息")
        {
            var returnMsg = new BaseMessage
            {
                CurrentConnectionId = CurrentGroupUserConnectionId,
                BaseMessageType = MessageType.MessageTypeSystem
            };
            returnMsg.MessageDetail = new SystemMessage { SysMessage = message, SysName = title };
            return returnMsg;
        }
        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            return Clients.All.hubMessage(getSysMsg("连接服务器成功..."));
        }
        /// <summary>
        /// 重新连接服务器
        /// </summary>
        /// <returns></returns>
        public override Task OnReconnected()
        {
            //已与服务器重新连接...
            return Clients.All.hubMessage(getSysMsg("已与服务器重新连接..."));
        }
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            //更新在线用户数据
            UpdateOnlineUser();
            return Clients.All.hubMessage(getSysMsg("已与服务器断开连接..."));
        }

      
        #endregion

        #region 服务端发送消息方法
        /// <summary>
        /// 给某个用户(连接ID)发信息
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="message"></param>
        public void SendToClient(string connectionId, ChatMessage message)
        {
            Clients.Client(connectionId).hubMessage(message);
        }

        /// <summary>
        /// 发送消息给多个客户端 非组织
        /// </summary>
        /// <param name="connectionIds"></param>
        /// <param name="message"></param>
        public void SendToClients(IList<string> connectionIds, ChatMessage message)
        {
            Clients.Clients(connectionIds).hubMessage(message);
        }
        /// <summary>
        /// 发送多个消息给客户端
        /// </summary>
        /// <param name="connectionIds">string形式 connectionId0|connectionId1|connectionId2</param>
        /// <param name="message"></param>
        public void SendToClients(string connectionIds, ChatMessage message)
        {
            if (string.IsNullOrEmpty(connectionIds)) { return; }
            SendToClients(connectionIds.Split('|').ToList(), message);
        }
        /// <summary>
        /// 发送给所有人
        /// </summary>
        /// <param name="message"></param>
        public void SendToAll(ChatMessage message)
        {
            Clients.All.hubMessage(message);
        }
        public void SendToGroup(string groupName, BaseMessage message)
        {
            message.CurrentConnectionId = Context.ConnectionId;
            Clients.Group(groupName).hubMessage(message);
        }
        #region 创建会话连接
        /// <summary>
        /// 群组会话连接时执行的方法
        /// 可重写方法，执行连接到本组的其他方法
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="cvnumber"></param>
        public virtual void GroupToConnection(string groupName)
        {
            //当前组
            CurretnGroupName = groupName;
            //添加到当前组
            Groups.Add(CurrentGroupUserConnectionId, groupName);
            //添加在线人数（demo，可以是数据库或者其他缓存方式等）
            AddOnlineUser(groupName);

            ClientOnConnectedCallBack(getSysMsg(string.Format("新用户{0}加入当前组{1},当前本组用户数：{2}", CurrentGroupUserConnectionId, groupName, GroupOnLineUserCount)));
           
        }

        /// <summary>
        /// 可重写方法，查寻本组用户
        /// </summary>
        /// <param name="groupName"></param>
        public virtual void GetAllUsersByGroup(string groupName)
        {
            //SendToGroup(groupName, new BaseMessage
            //{
            //    Content = string.Format("当前用户数为：{0}人",GroupOnLineUserCount),
            //    CurrentConnectionId = CurrentGroupUserConnectionId,
            //    Name = "系统功能：查询用户数据"
            //});
        }
        #endregion

        #endregion

        #region 客户端事件
        /// <summary>
        /// 当前用户连接服务器成功,执行客户端回调
        /// </summary>
        /// <param name="message"></param>
        public void ClientOnConnectedCallBack(BaseMessage message)
        {
            Clients.Caller.clientOnConnectedCallBack(message);
        }
        #endregion

        #region 私有方法
        private void AddOnlineUser(string groupName)
        {
            if (ChatHub.OnLineUser == null) { ChatHub.OnLineUser = new List<OnlineUser>(); }
            ChatHub.OnLineUser.Add(new OnlineUser
            {
                clientUserId = CurrentGroupUserConnectionId,
                connectionId = CurrentGroupUserConnectionId,
                groupName = groupName
            });
        }

        /// <summary>
        /// 更新当前用户 可重写 未实验
        /// </summary>
        public virtual void UpdateOnlineUser()
        {
            if (ChatHub.OnLineUser == null) { return; }
            var removeClient = ChatHub.OnLineUser.FirstOrDefault(x => x.connectionId == CurrentGroupUserConnectionId);
            if (removeClient != null)
            {
                ChatHub.OnLineUser.Remove(removeClient);
            }
        }
        #endregion
    }
}