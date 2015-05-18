using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pz.ChatServer.Core
{
    internal class CacheKey
    {
        /// <summary>
        /// 每个组的成员信息key
        /// </summary>
        public const string KEY_GROUP_ALL_USER = "KEY_GROUP_ALL_USER_{0}";
        /// <summary>
        /// 所有用户未读信息key
        /// </summary>
        public const string KEY_USER_UNREAD_DATA = "KEY_USER_UNREAD_DATA";
    }
}