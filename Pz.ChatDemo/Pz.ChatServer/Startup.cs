using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Pz.ChatServer.Startup))]
namespace Pz.ChatServer {
    /// <summary>
    /// 
    /// </summary>
    public class Startup {
        public void Configuration(IAppBuilder app) {
            app.Map("/push/im", map => {
                var hubConfiguration = new HubConfiguration() {
                    EnableJSONP = true
                };
                map.RunSignalR(hubConfiguration);
            });
        }
    }
}
