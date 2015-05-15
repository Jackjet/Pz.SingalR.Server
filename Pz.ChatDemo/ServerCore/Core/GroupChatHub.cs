using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pz.ChatServer.Core
{
    public class GroupChatHub : ChatHub
    {
        public override void GroupToConnection(string groupName, string userId = "")
        {
            base.GroupToConnection(groupName, userId);
        }
    }
}