using Pz.ChatServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pz.ChatServer.Core
{
    /// <summary>
    /// 操作未读消息接口
    /// </summary>
   public interface IAnaylizeUnRead<T> where T:class,new()
    {
       /// <summary>
       /// 添加未读消息信息
       /// 人员筛选业务包括在内，将已在线的去掉
       /// </summary>
       /// <param name="messageType">消息类型</param>
       /// <param name="clientUserIds">接收到的人员Id</param>
       ///<param name="clientUserIds">组织Id</param>
       /// <returns>返回操作成功True 操作失败False</returns>
       bool AddUserToUserMsg(MessageType messageType,List<string> clientUserIds,string groupId);
       /// <summary>
       /// 消息推送到组织添加未读
       /// 根据组织Id操作内部业务逻辑，获取未在线名单
       /// </summary>
       /// <param name="messageType">消息类型</param>
       /// <param name="groupId">组织Id</param>
       /// <returns>返回操作成功True 操作失败False</returns>
       bool AddUserToGroupMsg(MessageType messageType, string groupId);
       /// <summary>
       /// 更新某个用户的未读消息
       /// </summary>
       /// <param name="messageType">消息类型</param>
       /// <param name="clientUserId">用户ID</param>
       /// <param name="count">count+=count</param>
       /// <returns></returns>
       bool UpdateUserUnReadCount(MessageType messageType,string clientUserId,int count = 0);
       bool UpdateUserUnReadCount(T oldMsg, T newMsg);
       /// <summary>
       /// 清空某个用户的未读消息
       /// </summary>
       /// <param name="clientUserId">用户ID</param>
       /// <returns>返回操作成功True 操作失败False</returns>
       bool ClearUserAllUnReadInfo(string clientUserId);
       /// <summary>
       /// 清空某个用户在某个组织的未读消息
       /// </summary>
       /// <param name="clientUserId">用户ID</param>
       /// <param name="groupId">组织ID</param>
       /// <returns>返回操作成功True 操作失败False</returns>
       bool ClearUserGroupUnReadInfo(string clientUserId, string groupId);
    }
}
