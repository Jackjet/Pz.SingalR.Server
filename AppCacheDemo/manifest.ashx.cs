using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebApplicationCache2.ashx
{
    /// <summary>
    /// manifest 的摘要说明
    /// </summary>
    public class manifest : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //必须设置缓存文件类型为 text/cache-manifest
            context.Response.ContentType = "text/cache-manifest";
            StringBuilder str = new StringBuilder();
            string version = context.Request["v"]??"main";
            str.Append("CACHE MANIFEST\n");
            //版本号，更新manifest文件时候，可以通过更新版本号更新缓存
            str.AppendFormat("#v{0}1.1.6\n", version);
            //需要缓存的文件
            str.Append("CACHE:\n");
            //此处GetCacheFiles方法可以做很多扩展，例如通过配置得到相应的需要缓存的文件，可以根据不同页面得到不同的（需要缓存的）文件列表
            str.Append(GetCacheFiles(version));
            //网络请求的文件，不需要缓存
            str.Append("NETWORK:\n");
            str.Append(GetUnCacheFiles(version)); 
            //无法访问的页面
            str.Append("FALLBACK:\n");
            context.Response.Write(str.ToString());
        }
        /// <summary>
        /// 根据版本号（参数）得到相应的需要网络请求的文件资源连接
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        private string GetUnCacheFiles(string version)
        {
            StringBuilder str = new StringBuilder();
            switch (version)
            {
                //主页面的配置参数，返回相应的文件列表
                case "main":
                   
                    break;
                case "detail":
                    str.Append("/home/getdata\n");
                    break;
                //静态页面的配置参数
                case "html":
                    break;
                case "index":
                    str.Append("/images/uncache1.jpg\n");
                    str.Append("/images/uncache2.jpg\n");
                    str.Append("/images/uncache3.jpg\n");
                    break;
                default:
                    break;
            }

            return str.ToString();
        }
        /// <summary>
        /// 根据版本号（参数）获取需要缓存的资源连接
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        private string GetCacheFiles(string version)
        {
            //return "/index.html\n/demo.js\n";
            StringBuilder str = new StringBuilder();
            switch (version)
            { 
                    //主页面的配置参数，返回相应的文件列表
                case "main":
                case "detail":
                    //DEMO网页的css
                    str.Append("/Content/css?v=m8KdMFOCcNeZrATLbCQ_9gxex1_Ma7rE5iJzJXojUIk1\n");
                    //DEMO网页的modernizr
                    str.Append("/bundles/modernizr?v=wBEWDufH_8Md-Pbioxomt90vm6tJN2Pyy9u9zHtWsPo1\n");
                    //DEMO网页的jquery
                    str.Append("/bundles/jquery?v=FVs3ACwOLIVInrAl5sdzR2jrCDmVOWFbZMY6g6Q0ulE1\n");
                    //DEMO网页的bootstrap
                    str.Append("/bundles/bootstrap?v=2Fz3B0iizV2NnnamQFrx-NbYJNTFeBJ2GM05SilbtQU1\n");
                    break;
                    //静态页面的配置参数
                case "html":
                    //此行不加也可以，在页面的html  <html manifest="/ashx/manifest.ashx?v=html">即可
                    //（由于是demo，所以直接写死了，开发中可以通过配置灵活应用）
                    //此图是config.html页面的源码截图，将图片缓存（图片缓存比较常用）
                    str.Append("/images/ashxdemo.png\n");
                   // str.Append("/config.html");
                    break;
                case "index":
                    str.Append("/images/u=2474919563,1297370200&fm=21&gp=0.jpg\n");
                    str.Append("/images/u=3051795107,605456499&fm=21&gp=0.jpg\n");
                    str.Append("/images/u=3939224,1815707626&fm=21&gp=0.jpg\n");
                    str.Append("/images/duibi.png\n");
                    break;
                default:
                    break;
            }
           
            return str.ToString();
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}