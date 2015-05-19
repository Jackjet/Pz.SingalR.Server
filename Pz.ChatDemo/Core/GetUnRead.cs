using Macrosage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macrosage.BLL.Core
{
    public class GetUnRead:IGetUnRead<UnReadMsg>
    {
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

        public List<UnReadMsg> GetUserUnReadData(string clientUserId)
        {
            //从缓存里面读取
            return new PushCache().GetUnReadDataFromCacheByPredicate(x => x.clientUserId == clientUserId);
        }
    }
}
