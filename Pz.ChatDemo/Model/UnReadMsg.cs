using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pz.ChatServer.Model
{
    public class UnReadMsg:BaseGroup
    {
        public string clientUserId { get; set; }
        public MessageType MessageType { get; set; }
        public int UnReadCount { get; set; }
    }
}