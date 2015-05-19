using Macrosage.Model;
using Macrosage.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macrosage.BLL.Core
{
   public class PushQueue
    {
       private const int QueueID = 1003;
       private const int QueueMaxCount = 100;
       public MsQueue msQueue
       {
           get { return MsQueue.Instance(QueueID); }
       }
       /// <summary>
       /// 将未读消息添加到队列
       /// </summary>
       /// <param name="unReadMsg"></param>
       /// <returns></returns>
       public bool AddUnReadMsgToQueue(List<UnReadMsg> unReadMsg)
       {
           if (unReadMsg != null)
           {
               foreach (var item in unReadMsg)
               {
                   if (!msQueue.SendByBit<UnReadMsg>(item))
                   {
                       return false;
                   }
               }
               return AddUnReadMsgToDbFromQueue();
           }
           return true;
       }
       /// <summary>
       /// 将队列数据添加到数据库里去
       /// </summary>
       /// <returns></returns>
       public bool AddUnReadMsgToDbFromQueue(int num = QueueMaxCount)
       {
           int count = msQueue.QueueMessageCount;
           List<UnReadMsg> unreadReal = new List<UnReadMsg>();
           if (count >= num)
           {
               //将所有队列里的未读数据读取出来
               for (int i = 0; i < count; i++)
               {
                   var model = msQueue.ReceiveByBit<UnReadMsg>();
                   //如果列表里面没有那条数据，就添加，否则，就更新加1
                   var first =unreadReal.FirstOrDefault(x => x.clientUserId == model.clientUserId && x.GroupId == model.GroupId && x.MessageType == model.MessageType) ;
                   if (first == null)
                   {
                       unreadReal.Add(model);
                   }
                   else
                   {
                       first.UnReadCount++;
                   }

               }
             //添加到数据库
             return  UnReadDataBLL.Instance.AddUnReadData(unreadReal);
           }
           return true;
       }
    }
}
