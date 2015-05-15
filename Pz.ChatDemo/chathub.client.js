
/*
    @author:fanyuepan
    @createtime:2015-05-15
    @description:chathub client js 
    @v1.0
*/
(function ($) {
    if(typeof $ == 'undefined'){ alert('请引用jquery.');}
    var chathub = {
        option: {
            serverUrl: '',
            userId: 0,
            groupId: 0,
            receiveCallBack: function (result) {
                console.log("你收到了新消息：" + result);
            }
        },
        proxy: {
            proxyNormal: null,//普通聊天室内
            proxyAdmin: null //实时监测聊天室（预留）
        },
        messageType: {
            messageTypeNormal: 1,
            messageTypeSystem: 2,
            messageTypeArticle: 3,
            messageTypeNormalAnnex: 4,
            messageTypePersonalEmail:5
        },
        client: {
            init: function () {
                _this.proxy.proxyNormal.client.hubMessage = function (result) {
                    console.log(result);
                    _this.option.receiveCallBack(result);
                };
            },
            send: function (msg, username) {
                var obj = {
                    messagetype: _this.messageType.messageTypeNormal,
                    userid: _this.option.userId,
                    body: {
                        name: username,
                        message: msg
                    }
                };
                _this.proxy.proxyNormal.server.sendToGroup(_this.option.groupId, obj);
            }
        },
        init: function (option) {
            $.extend(_this.option, option);
            _this.server.init();//服务端代码初始化  
            _this.client.init();//客户端代码初始化
         
        },
        server: {
            init: function () {
                this.connect();
                _this.proxy.proxyNormal.client.clientOnConnectedCallBack = this.connectCallBack;
            },
            connect: function () {
                $.connection.hub.url = _this.option.serverUrl;
                _this.proxy.proxyNormal = $.connection.chatHub;
                //_this.proxy.proxyAdmin = $.connection.adminHub
                $.connection.hub.start({ jsonp: true }).done(function () {
                    //连接服务器
                    _this.proxy.proxyNormal.server.groupToConnection(_this.option.groupId, _this.option.userId);
                    console.log('连接成功');
                }).fail(function () {
                    console.log("连接失败");
                });
            },
            connectCallBack: function (result) {
                console.log(result);
            }
        }
    };
    var _this = chathub;
    window.chatClient = _this;
})($);
