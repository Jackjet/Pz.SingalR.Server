using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pz.ChatServer.Model
{
    public class UnReadMsgV1 : BaseGroup
    {
        public string clientUserId { get; set; }
        public List<UnReadMsg> UnReadMsgList { get; set; }
    }
}