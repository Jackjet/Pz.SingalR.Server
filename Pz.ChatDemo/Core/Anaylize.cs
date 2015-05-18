using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pz.ChatServer.Model;

namespace Pz.ChatServer.Core
{
    public class Anaylize :IAnaylizeOnlineUser<OnlineUser>
    {
        #region 私有变量和方法
        private IList<OnlineUser> _onlineUser;
        /// <summary>
        /// 当前是否有在线的人
        /// </summary>
        private bool hasOnLineUser
        {
            get
            {
                if (_onlineUser == null || _onlineUser.Count < 1)
                {
                    return false;
                }
                return true;
            }
        }
        /// <summary>
        /// 私有方法，获取某个组的在线人员列表
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        private IList<OnlineUser> PredicateOnlineUser(string groupId)
        {
            var list = _onlineUser.Where(x => x.groupName == groupId);
            if (list == null)
            {
                return new List<OnlineUser>();
            }
            return list.ToList();
        }

        #endregion

        #region 很简单，实现接口方法
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="onlineUser"></param>
        public Anaylize(IList<OnlineUser> onlineUser)
        {
            setOnlineUser(onlineUser);
        }
        /// <summary>
        /// 设置在线人员列表
        /// </summary>
        /// <param name="onlineUser"></param>
        public void setOnlineUser(IList<OnlineUser> onlineUser)
        {
            _onlineUser = onlineUser;
        }

       /// <summary>
       /// 获取在线人员用户个数
       /// </summary>
       /// <param name="groupId"></param>
       /// <returns></returns>
        public int GetOnlineUserCount(string groupId)
        {
            return PredicateOnlineUser(groupId).Count;
        }
        /// <summary>
        /// 获取在线用户ID列表
        /// </summary>
        /// <param name="groupId">组唯一标识</param>
        /// <returns></returns>
        public IList<string> GetOnlineUserIds(string groupId)
        {
            return PredicateOnlineUser(groupId).Select(x => x.clientUserId).ToList();
        }
        /// <summary>
        /// 获取在线用户列表
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public IList<OnlineUser> GetGourpOnlineUsers(string groupId)
        {
            return PredicateOnlineUser(groupId);
        }
        /// <summary>
        /// 获取在线总人数
        /// </summary>
        /// <returns></returns>
        public int GetAllOnlineUserCount()
        {
            return hasOnLineUser ? _onlineUser.Count : 0;
        }
        /// <summary>
        /// 获取在线用户ID列表
        /// </summary>
        /// <returns></returns>
        public IList<string> GetOnlineUserIds()
        {
            return hasOnLineUser ? _onlineUser.Select(x => x.clientUserId).ToList() : null;
        }
        /// <summary>
        /// 获取所有在线用户列表
        /// </summary>
        /// <returns></returns>
        public IList<OnlineUser> GetAllOnlineUsers()
        {
            return _onlineUser;
        }
        #endregion

        /// <summary>
        /// 判断某个用户是否在线
        /// </summary>
        /// <param name="clientUserId">用户Id</param>
        /// <returns></returns>
        public bool IsOnline(string clientUserId)
        {
            return hasOnLineUser ? _onlineUser.Any(x => x.clientUserId == clientUserId) : false;
        }

        /// <summary>
        /// 判断某个用户是否组内在线
        /// </summary>
        /// <param name="clientUsrId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public bool IsGroupOnline(string clientUserId, string groupId)
        {
            return hasOnLineUser ? _onlineUser.Any(x => x.clientUserId == clientUserId && x.groupName == groupId) : false;
        }
    }
}