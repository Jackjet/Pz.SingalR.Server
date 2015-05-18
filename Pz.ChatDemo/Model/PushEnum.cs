using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pz.ChatServer.Model
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
}