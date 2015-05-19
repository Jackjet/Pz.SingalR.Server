using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Macrosage.BLL.Core
{
    public class GroupChatHub : ChatHub
    {
        public override void GroupToConnection(string groupName, string userId = "")
        {
            base.GroupToConnection(groupName, userId);
        }
    }
}