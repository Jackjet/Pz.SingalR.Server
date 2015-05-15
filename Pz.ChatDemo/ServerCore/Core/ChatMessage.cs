using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Pz.ChatServer.Core
{
    public enum MessageType
    {
        /// <summary>
        /// 普通消息类型
        /// </summary>
        MessageTypeNormal = 1,
        /// <summary>
        /// 文章消息类型
        /// </summary>
        MessageTypeArticle = 2,
        /// <summary>
        /// 带附件消息类型
        /// </summary>
        MessageTypeNormalAnnex = 3,
        /// <summary>
        /// 通知消息类型
        /// </summary>
        MessageTypeNotification = 4,
        /// <summary>
        /// 系统消息类型
        /// </summary>
        MessageTypeSystem = 5
    }
    public class BaseMessage
    {
        [JsonProperty("connectionid")]
        /// <summary>
        /// 当前用户连接ID
        /// </summary>
        public string CurrentConnectionId { get; set; }
        [JsonProperty("userid")]
        public string CurrentUserId { get; set; }
        [JsonProperty("messagetype")]
        public MessageType BaseMessageType { get; set; }
        [JsonProperty("addtime")]
        public DateTime AddTime { get { return DateTime.Now; } }
        private object _messageDetail;
        [JsonProperty("body")]
        public object MessageDetail
        {
            get { return _messageDetail; }
            set
            {
                _messageDetail = value;
                //switch (BaseMessageType)
                //{ 
                //    case MessageType.MessageTypeArticle:
                //        //_messageDetail = new ChatArticleMessage();
                //        break;
                //    case MessageType.MessageTypeNormal:
                //        _messageDetail = new ChatMessage();
                //        break;
                //    case MessageType.MessageTypeNormalAnnex:
                //       // _messageDetail = new ChatAnnexMessage();
                //        break;
                //    case MessageType.MessageTypeNotification:
                //       // _messageDetail = new ChatNotificationMessage();
                //        break;
                //    case MessageType.MessageTypeSystem:
                //        _messageDetail = new SystemMessage();
                //        break;
                //    default:
                //        _messageDetail = new SystemMessage();
                //        break;
                //}
            }
        }
    }
    public class SystemMessage
    {
        [JsonProperty("name")]
        public string SysName { get; set; }
        [JsonProperty("message")]
        public string SysMessage { get; set; }
    }
    public class ChatMessage 
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string Time { get { return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); } }
    }

    public class OnlineUser
    {
        [JsonProperty("group_id")]
        /// <summary>
        /// 用户客户端组ID
        /// </summary>
        public string groupName { get; set; }
        [JsonProperty("connection_id")]
        /// <summary>
        /// 连接用户ID
        /// </summary>
        public string connectionId { get; set; }
        [JsonProperty("client_userid")]
        /// <summary>
        /// 用户客户端ID
        /// </summary>
        public string clientUserId { get; set; }
    }
}